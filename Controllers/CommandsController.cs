using System.Collections.Generic;
using AutoMapper;
using CommandBook.Data;
using CommandBook.DTO;
using CommandBook.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CommandBook.Controllers {
    //decorating our Controller
    //api/commands
    //[Route("api/[controller]")] is also possible
    //[Route("api/commands")]
   //[ApiController] decorator has so many support always use this decorator

    [ApiController]
    public class CommandsController:ControllerBase{

        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;


        //CommandsController:ControllerBase inherit controller without view support
        //CommandsController:ControllerBase would inherit controller with view

        public CommandsController(ICommanderRepo repository,IMapper mapper)
         {
             _repository = repository;
             _mapper = mapper;
         }


        //get api/commands
        //[Route("api/commands")]
        [HttpGet]
        [Route("api/commands")]
        public ActionResult <IEnumerable<CommandReadDto>> GetAllCommands(){
            var items = _repository.GetAllCommands();
            
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(items));
           
        }

        //get api/commands/{id}
        
        [HttpGet("{id}",Name="GetCommandById")]
        [Route("api/commands/{id}")]
        public ActionResult <CommandReadDto> GetCommandById(int id){
         var item = _repository.GetCommandById(id);
         if(item !=null){
            
            return Ok(_mapper.Map<CommandReadDto>(item));
         } 
         //returns 404 not found
          return NotFound();  
         
         
        }
        [HttpPost] 
        [Route("api/commands/create")]
        public ActionResult <CommandReadDto> CreateCommand(CommandCreateDto cmd){
            if(cmd !=null){
               var commandModel =  _mapper.Map<Command>(cmd);
               _repository.CreateCommand(commandModel);
               _repository.SaveChanges();

               var commandReadDto =  _mapper.Map<CommandReadDto>(commandModel);

               //rest api best practie to return item creation location

               return CreatedAtRoute(nameof(GetCommandById),new {Id = commandReadDto.Id },commandReadDto);

            }

            return NotFound();
        }




        [HttpPut("{id}")]
        [Route("api/commands/update/{id}")]
        public ActionResult UpdateCommand(int id,CommandUpdateDto cmd){
            var item = _repository.GetCommandById(id);
            if(item == null){
                return NotFound();

            }
            //_mapper.Map(source,destination);
            _mapper.Map(cmd,item);
            _repository.UpdateCommand(item);
            _repository.SaveChanges();
             //returns 204
            return NoContent();


        }

        //patch request allows 6 operation Add | Remove |Replace | Copy | Move | Test
        // for the purpose of this project i have chose to use Replace operation to update 
        // its syntax looks like 
        //   [
        //     {
        //       "op":"replace",
        //       "path":"/line",
        //      "value":"Add-Migrations <migration name> || dotnet ef add-migration"
        //     }
        //   ]

        // opt is the operation name either of the 6
        // path is the data element we want to partially update
        // value is the new ValueResolverConfiguration we wants to update
        
        [HttpPatch("{id}")]
        [Route("api/commands/partialupdate/{id}")]
        public ActionResult PartialUpdateCommand(int id,JsonPatchDocument<CommandUpdateDto> patchDocument){
         var item = _repository.GetCommandById(id);
            if(item == null){
                return NotFound();

            }

            var itemToPatch = _mapper.Map<CommandUpdateDto>(item);
            patchDocument.ApplyTo(itemToPatch,ModelState);
            if(!TryValidateModel(itemToPatch)){
                return ValidationProblem(ModelState);
            }
            //updating from CommandUpdateto to Command entity Model
            _mapper.Map(itemToPatch,item);
            _repository.UpdateCommand(item);
             _repository.SaveChanges();
             //returns 204
            return NoContent();


        }

      
      [HttpDelete("{id}")]
      [Route("api/commands/delete/{id}")]
      public ActionResult DeleteCommand(int id){
           var itemToDelete = _repository.GetCommandById(id);
             if(itemToDelete == null){
               return NotFound();
                }

             _repository.DeleteCommand(itemToDelete);
             _repository.SaveChanges();
             return NoContent();
      }

    }
}