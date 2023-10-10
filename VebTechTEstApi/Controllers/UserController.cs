using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VebTechTEstApi.ConstantsData;
using VebTechTEstApi.Services;
using VebTechTEstApi.ViewModels.UserViewModels;


namespace VebTechTEstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Получение списка пользователей.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /GetUsers
        ///     {
        ///         "page": { - пагинация
        ///                     "number": 2 - номер страницы
        ///                     "size": 20 - размер страницы
        ///                 }
        ///         "orderBy": { - сортировка
        ///                     "sortField": "Name" - Столбец для сортировки
        ///                     "ascending": true - вид сортировки
        ///                 }
        ///         "AgeFilter": { - фильтр по возрасту
        ///                     "min": 25 - минимальный возраст
        ///                     "max": 40 - максимальный возраст
        ///                 }  
        ///         "nameFilter": "Alex", - фильр по имени
        ///         "emailFilter": "superadmin@gmail.com", - фильтр по email
        ///         "roleFilter": [ - список ролей для фильтра
        ///                     "dcf75e7f-1a5b-4db0-9817-888d90c1509c"
        ///                     ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Список пользователей</returns>
        /// <response code="200">Вернёт список пользователей системы
        /// 
        ///     GET /GetUsers
        ///     {
        ///         "totalItems": 1,
        ///         "users": [
        ///             {
        ///                 "id": "44B2D7AF-BF9F-4A51-9EAE-3DFE153EF315",
        ///                 "name": "SuperAdmin",
        ///                 "age": 31,
        ///                 "email": "superadmin@gmail.com",
        ///                 "roles": [
        ///                     "SuperAdmin"
        ///                     ]
        ///            }   
        ///        ]
        ///     }
        /// </response>
        /// <response code="400">Неверные данные фильтра</response>
        [Authorize]
        [HttpGet("Get")]
        public async Task<ObjectResult> GetUsers([FromQuery] IndexViewModel model)
        {
            try
            {
                var result = await _userService.GetUsersAsync(model);
                if(result == null) return BadRequest("Неверные параметры!");
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Создание пользователя.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Create
        ///     {
        ///         "name": "Alex", - Имя
        ///         "age": 31, - Возраст
        ///         "email": "superadmin@gmail.com", - электронный адресс
        ///         "password": "11111111" - пароль
        ///         "roles": [ - список ролей
        ///                     "dcf75e7f-1a5b-4db0-9817-888d90c1509c"
        ///                     ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Список пользователей</returns>
        /// <response code="201">Вернёт список пользователей системы
        /// 
        ///     POST /Create
        ///     {
        ///         "totalItems": 1,
        ///         "users": [
        ///             {
        ///                 "id": "44B2D7AF-BF9F-4A51-9EAE-3DFE153EF315",
        ///                 "name": "SuperAdmin",
        ///                 "age": 31,
        ///                 "email": "superadmin@gmail.com",
        ///                 "roles": [ - список ролей
        ///                     "SuperAdmin"
        ///                     ]
        ///            }   
        ///        ]
        ///     }
        /// </response>
        /// /// <response code="404">Пользователь не найден</response>
        /// <response code="400">Неверные данные</response>
        [Authorize(Roles = nameof(DefaultRoles.SuperAdmin))]
        [HttpPost("Create")]
        public async Task<ObjectResult> CreateUser(UserCreateViewModel model)
        {
            try
            {
                if (model.Name == null || model.Age == null || model.Email == null || model.Password == null)
                {
                    return BadRequest("Name, Age, Email, Password являются обязательными!");
                }
                if (model.Age <= 0)
                {
                    return BadRequest("Некорректный возраст!");
                }
                if (await _userService.CheckEmailAsync(model.Email))
                {
                    return BadRequest("Пользователь с таким email уже существует!");
                }
                if (model.Password.Length < 7)
                {
                    return BadRequest("Длина пароля должна быть не менее 8 символов!");
                }

                var result = await _userService.CreateUserAsync(model);
                
                return result == null ? NotFound(result) : StatusCode(201, result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /Delete
        ///     {
        ///        "id": "44B2D7AF-BF9F-4A51-9EAE-3DFE153EF315" - id пользователя
        ///     }
        ///
        /// </remarks>
        /// <returns>Удаление пользователя</returns>
        /// <response code="200">Вернёт сообщение об успешном удалении
        /// </response>
        /// <response code="400">Неверные данные</response>
        [Authorize(Roles = nameof(DefaultRoles.SuperAdmin))]
        [HttpDelete("Delete")]
        public async Task<ObjectResult> DeleteUser(string userId)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(userId);
                return result == 1 ? Ok("Пользователь удалён!") : NotFound("Пользователь не найден!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Получение данных о пользователе.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /GetOne
        ///     {
        ///        "id": "44B2D7AF-BF9F-4A51-9EAE-3DFE153EF315" - id пользователя
        ///     }
        ///
        /// </remarks>
        /// <returns>Данные пользователя</returns>
        /// <response code="200">Вернёт данные пользователя
        /// 
        ///     GET /GetOne
        ///     {
        ///         "id": "44B2D7AF-BF9F-4A51-9EAE-3DFE153EF315",
        ///         "name": "SuperAdmin",
        ///         "age": 31,
        ///         "email": "superadmin@gmail.com",
        ///         "roles": [
        ///             "SuperAdmin"
        ///             ]
        ///     }   
        /// </response>
        /// <response code="400">Неверные данные</response>
        [Authorize]
        [HttpGet("GetOne")]
        public async Task<ObjectResult> GetById(string userId)
        {
            try
            {
                var result = await _userService.GetByIdAsync(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Добавление роли пользователю.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /AddRole
        ///     {
        ///        "id": "44B2D7AF-BF9F-4A51-9EAE-3DFE153EF315" - id пользователя
        ///        "roles":[
        ///             "e3d139ee-f16b-4310-8a3b-8c8ec1437266" - id роли
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Данные пользователя</returns>
        /// <response code="200">Данные пользователя
        /// 
        ///     PATCH /AddRole
        ///     {
        ///         "id": "44B2D7AF-BF9F-4A51-9EAE-3DFE153EF315",
        ///         "name": "SuperAdmin",
        ///         "age": 31,
        ///         "email": "superadmin@gmail.com",
        ///         "roles": [
        ///             "SuperAdmin"
        ///             ]
        ///     } 
        /// </response>
        /// <response code="400">Неверные данные</response>
        [Authorize(Roles = nameof(DefaultRoles.SuperAdmin))]
        [HttpPatch("AddRole")]
        public async Task<ObjectResult> AddRole(UserAddRoleViewModel model)
        {
            try
            {
                var result = await _userService.AddRolesForUserAsync(model);
                if (result == null) return BadRequest("Неверные данные!");
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Обновление данных пользователя.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /Update
        ///     {
        ///        "id": "44B2D7AF-BF9F-4A51-9EAE-3DFE153EF315" - id пользователя
        ///         "name": "Alex", - Имя
        ///         "age": 31, - Возраст
        ///         "email": "superadmin@gmail.com", - электронный адресс
        ///     }
        ///
        /// </remarks>
        /// <returns>Данные пользователя</returns>
        /// <response code="200">Обновлённые данные пользователя
        /// 
        ///     PATCH /Update
        ///     {
        ///         "id": "44B2D7AF-BF9F-4A51-9EAE-3DFE153EF315",
        ///         "name": "SuperAdmin",
        ///         "age": 31,
        ///         "email": "superadmin@gmail.com",
        ///         "roles": [
        ///             "SuperAdmin"
        ///             ]
        ///     } 
        /// </response>
        /// <response code="400">Неверные данные</response>
        [Authorize(Roles = $"{nameof(DefaultRoles.SuperAdmin)}, {nameof(DefaultRoles.Admin)}")]
        [HttpPatch("Update")]
        public async Task<ObjectResult> UpdateUser(UserUpdateViewModel model)
        {
            try
            {
                if (model.Name == null || model.Age == null || model.Email == null)
                {
                    return BadRequest("Name, Age, Email, Password являются обязательными!");
                }
                if (model.Age <= 0)
                {
                    return BadRequest("Некорректный возраст!");
                }
                if (await _userService.CheckEmailAsync(model.Email))
                {
                    return BadRequest("Пользователь с таким email уже существует!");
                }

                var result = await _userService.UpdateUserAsync(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
