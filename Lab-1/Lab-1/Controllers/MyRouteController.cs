using Lab_1.BLL;
using Lab_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyRouteController : ControllerBase
    {
        private readonly Manager manager;

        public MyRouteController(Manager manager)
        {
            this.manager = manager;
        }

        // POST: api/Flights
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Lab_1.Models.Route>> AddRoute([FromBody] Lab_1.Models.Route route)
        {
            return await manager.AddRoute(route) ? CreatedAtAction(nameof(GetRoute), new { routeName = route.RouteName }, route) : BadRequest("failed to add flight");
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lab_1.Models.Route>>> GetRoutes()
        {
            var route = await manager.GetRoutes();
            if (route == null)
                return NotFound();
            return route;
        }

        // GET: api/Flights/5
        [HttpGet("{routeName}")]
        public async Task<ActionResult<Lab_1.Models.Route>> GetRoute(string routeName)
        {
            var route = await manager.GetRoute(routeName);
            if (route == null)
                return NotFound();
            return route;
        }

        // PUT: api/Flights
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutRoute([FromBody] Lab_1.Models.Route route)
        {
            return await manager.UpdateRoute(route) ? Ok() : NotFound();
        }

        // DELETE: api/Flights/5
        [HttpDelete("{routeName}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRoute(string routeName)
        {
            return await manager.DeleteRoute(routeName) ? Ok() : NotFound();
        }
    }
}
