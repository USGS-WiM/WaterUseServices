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
using WiM.Security.Authentication.Basic;

namespace WaterUseServices.XUnitTest
{
    public class CatagoriesTest
    {
        public CategoriesController controller { get; private set; }
        public CatagoriesTest() {
            //Arrange
            controller = new CategoriesController(new InMemoryAgent());
            //must set explicitly for tests to work
            controller.ObjectValidator = new InMemoryModelValidator();


        }
        [Fact]
        public async Task GetAll()
        {           

            //Act
            var response = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var result = Assert.IsType<EnumerableQuery<CatagoryType>>(okResult.Value);

            Assert.Equal(2, result.Count());
            Assert.Equal("MockTestCatagory2", result.LastOrDefault().Name);
            Assert.Equal("test mock catagory 2", result.LastOrDefault().Description);
        }

        [Fact]
        public async Task Get()
        {
            //Arrange
            var id = 1;

            //Act
            var response = await controller.Get(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var result = Assert.IsType<CatagoryType>(okResult.Value);
            
            Assert.Equal("MockTestCatagory1", result.Name);
            Assert.Equal("test mock catagory 1", result.Description);
        }

        [Fact]
        public async Task Post()
        {
            //Arrange
            var entity = new CatagoryType() {Name = "newCatagory", Description = "New mock catagory 3" };
   

            //Act
            var response = await controller.Post(entity);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var result = Assert.IsType<Role>(okResult.Value);
            

            Assert.Equal("newRole", result.Name);
            Assert.Equal("New mock role 3", result.Description);
        }

        [Fact]
        public async Task Put()
        {
            //Arrange
            var get = await controller.Get(1);
            var okgetResult = Assert.IsType<OkObjectResult>(get);
            var entity = Assert.IsType<CatagoryType>(okgetResult.Value);

            entity.Name = "editedName";

            //Act
            var response = await controller.Post(entity);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var result = Assert.IsType<CatagoryType>(okResult.Value);

            Assert.Equal(entity.Name, result.Name);
            Assert.Equal(entity.Description, result.Description);
            Assert.Equal(entity.ID, result.ID);
        }

        [Fact]
        public async Task Delete()
        {
            //Act
            await controller.Delete(1);

            var response = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response);
            var result = Assert.IsType<EnumerableQuery<Role>>(okResult.Value);

            Assert.Equal(1, result.Count());
            Assert.Equal("MockTestCatagory2", result.LastOrDefault().Name);
            Assert.Equal("test mock catagory 2", result.LastOrDefault().Description);
        }
    }
    
}
