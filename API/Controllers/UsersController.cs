using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    [HttpGet] // /api/users
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetMembersAsync();

        // var mappedUsers = _mapper.Map<IEnumerable<MemberDto>>(users);

        // return Ok(mappedUsers);
        return Ok(users);
    }
    [HttpGet("{username}")] // /api/users/{username}
    public async Task<ActionResult<MemberDto>> GetUser([FromRoute] string username)
    {

        return await _userRepository.GetMemberAsync(username);

    }

}
