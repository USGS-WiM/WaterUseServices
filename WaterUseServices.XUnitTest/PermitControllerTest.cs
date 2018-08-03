//Unit testing involves testing a part of an app in isolation from its infrastructure and dependencies. 
//When unit testing controller logic, only the contents of a single action is tested, not the behavior of 
//its dependencies or of the framework itself. As you unit test your controller actions, make sure you focus 
//only on its behavior. A controller unit test avoids things like filters, routing, or model binding. By focusing 
//on testing just one thing, unit tests are generally simple to write and quick to run. A well-written set of unit 
//tests can be run frequently without much overhead. However, unit tests do not detect issues in the interaction 
//between components, which is the purpose of integration testing.

using System;
using Xunit;
using System.Threading.Tasks;
using WaterUseAgent;
using WaterUseServices.Controllers;
using Microsoft.AspNetCore.Mvc;
using WaterUseDB.Resources;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using WaterUseAgent.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WaterUseServices.XUnitTest
{
    public class PermitTest
    {
        public PermitsController controller { get; private set; }
        public PermitTest() {
            //Arrange
            controller = new PermitsController(new InMemoryAgent());
            //must set explicitly for tests to work
            controller.ObjectValidator = new InMemoryModelValidator();
        }
        

        [Fact]
        public async Task Get()
        {
            //Arrange
            var id = 1;

            //Act
            var response = await controller.GetSourcePermit(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var result = Assert.IsType<Permit>(okResult.Value);
            
            Assert.Equal("MockTestRole1", result.PermitNO);
        }

        [Fact]
        public async Task Post()
        {
            //Arrange
            var entity = new Permit() {PermitNO = "newPermitNo"};
   

            //Act
            var response = await controller.Post(1, entity);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var result = Assert.IsType<Permit>(okResult.Value);
            

            Assert.Equal("newPermitNo", result.PermitNO);
        }

        [Fact]
        public async Task Put()
        {
            //Arrange
            var get = await controller.GetSourcePermit(1);
            var okgetResult = Assert.IsType<OkObjectResult>(get);
            var entity = Assert.IsType<Permit>(okgetResult.Value);

            entity.PermitNO = "editedPermitNo";

            //Act
            var response = await controller.Put(1, entity);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var result = Assert.IsType<Permit>(okResult.Value);

            Assert.Equal(entity.PermitNO, result.PermitNO);
            Assert.Equal(entity.ID, result.ID);
        }

        [Fact]
        public async Task Delete()
        {
            //Act
            await controller.Delete(1);

            var response = await controller.GetSourcePermit(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var result = Assert.IsType<EnumerableQuery<Permit>>(okResult.Value);

            Assert.Equal(1, result.Count());
            Assert.Equal("MockTestPermit2", result.LastOrDefault().PermitNO);
        }
    }
}
