using Infrastructure.AnswerObjects;
using Infrastructure;
using Infrastructure.Dto;

namespace Infrastructure.Interfaces;

public interface IServerCommandService
{
    public Task<ResultModel<string, Exception>> Login(ServerModel serverModel);
    
    public Task<ResultModel<object, Exception>> AddVpnServer(ServerModel serverModel);
    
    public Task<ResultModel<object, Exception>> RemoveVpnServer(ServerModel serverModel);
    
    public Task<ResultModel<object, Exception>> GetVpnServer(ServerModel serverModel);
    
    public Task<ResultModel<string, Exception>> AddPersonToServer(ServerModel serverModel, UserModel userModel, DateTime? date = null);
    
    public Task<ResultModel<object, Exception>> RemovePersonFromServer(ServerModel serverModel, UserModel userModel);
    
    public Task<ResultModel<object, Exception>> UpdatePersonFromServer(ServerModel serverModel, UserModel userModel, DateTime? date = null);
    
    public Task<ResultModel<ICollection<UserModel>,Exception>> GetUsers(ServerModel serverModel);
}