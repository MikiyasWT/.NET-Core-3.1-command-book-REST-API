using System.Collections.Generic;
using CommandBook.Models;

namespace CommandBook.Data {
    public interface ICommanderRepo {
      
      IEnumerable<Command> GetAllCommands();
      Command GetCommandById(int id);

      void CreateCommand(Command cmd);

      bool SaveChanges();

      void UpdateCommand(Command cmd);

      void DeleteCommand(Command cmd);



     }
}