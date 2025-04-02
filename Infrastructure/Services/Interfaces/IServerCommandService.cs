using Infrastructure.AnswerObjects;
using Infrastructure.Dto;

namespace Infrastructure.Interfaces;

public interface IServerCommandService
{
    public Task<ResultModel<string, Exception>> Login(ServerDto serverDto);
    
    public Task<ResultModel<object, Exception>> AddVpnServer(ServerDto serverDto);
    
    public Task<ResultModel<object, Exception>> RemoveVpnServer(ServerDto serverDto);
    
    public Task<ResultModel<object, Exception>> GetVpnServer(ServerDto serverDto);
    
    public Task<ResultModel<string, Exception>> AddPersonToServer(ServerDto serverDto, UserDto userDto, DateTime? date = null);
    
    public Task<ResultModel<object, Exception>> RemovePersonFromServer(ServerDto serverDto, UserDto userDto);
    
    public Task<ResultModel<object, Exception>> UpdatePersonFromServer(ServerDto serverDto, UserDto userDto, DateTime? date = null);
    
    public Task<ResultModel<ICollection<UserDto>,Exception>> GetUsers(ServerDto serverDto);
}