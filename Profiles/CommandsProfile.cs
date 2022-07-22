using AutoMapper;
using CommandBook.DTO;
using CommandBook.Models;

namespace CommandBook.Profiles {
    public class CommandsProfile : Profile
    {
     public CommandsProfile()
     {
        //CreateMap<Source,target>();
        CreateMap<Command,CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();

        //map from CommandUpdateDto to Command Model
        CreateMap<CommandUpdateDto,Command>();
        //for patch update
        CreateMap<Command,CommandUpdateDto>();

        
     }
    }
}