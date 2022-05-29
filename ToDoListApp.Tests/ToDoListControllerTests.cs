using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using ToDoListApp.Client.Controllers;
using ToDoListApp.Client.Models;
using ToDoListApp.Domain.Interfaces;
using ToDoListApp.Domain.Models;

namespace ToDoListApp.Tests
{
    [TestFixture]
    public class ToDoListControllerTests
    {
        private Mock<IUnitOfWork> mockRepo;
        private Mock<ILogger<ToDoListController>> mockListLogger;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IUnitOfWork>();
            mockListLogger = new Mock<ILogger<ToDoListController>>();
        }

        [Test]
        public void Index_Returns_ViewResult_With_ToDoLists()
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.GetAllAsync().Result)
                    .Returns(GetTestToDoLists());
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);
            
            // Act
            var result = controller.Index().Result;
            
            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            mockRepo.Verify(x => x.ToDoLists.GetAllAsync(), Times.Exactly(1));
        }

        [Test]
        public void InvisibleLists_Returns_ViewResult_With_InvisibleToDoLists()
        {    
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.GetAllAsync().Result)
                    .Returns(GetTestToDoLists());
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);
            
            // Act
            var result = controller.InvisibleLists().Result;
            
            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            mockRepo.Verify(x => x.ToDoLists.GetAllAsync(), Times.Exactly(1));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Details_Returns_ViewResult_If_Model_Is_Returned(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns(GetTestToDoLists()[0]);
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);
            
            // Act
            var result = controller.Details(id, It.IsAny<bool>(), It.IsAny<bool>()).Result;
            
            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            mockRepo.Verify(x => x.ToDoLists.GetByIdAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Details_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called_If_Id_Passed_Is_Invalid (int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns(GetTestToDoLists()[0]);
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Details(id, It.IsAny<bool>(), It.IsAny<bool>()).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.GetByIdAsync(It.IsAny<int>()), Times.Exactly(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Details_Returns_RedirectToActionResult_And_Repository_Method_Is_Called_If_Model_Doesnt_exist(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns((ToDoList)null);
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Details(id, It.IsAny<bool>(), It.IsAny<bool>()).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.GetByIdAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        [Test]
        public void Create_Returns_ViewResult()
        {
            // Arrange
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);
            
            // Act
            var result = controller.Create();
            
            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        public void Create_Post_Returns_RedirectToActionResult()
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.Add(It.IsAny<ToDoList>()));
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);
            
            // Act
            var result = controller.Create(GetTestToDoListModels()[0]).Result;
            
            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.Add(It.IsAny<ToDoList>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(null)]
        public void Create_Post_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called_If_Model_Is_Null_Or_Invalid(ToDoListModel list)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.Add(It.IsAny<ToDoList>()));
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Create(list).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.Add(It.IsAny<ToDoList>()), Times.Exactly(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Edit_Returns_ViewResult_If_Model_Is_Returned(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns(GetTestToDoLists()[0]);
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);
            
            // Act
            var result = controller.Edit(id).Result;
            
            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            mockRepo.Verify(x => x.ToDoLists.GetByIdAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Edit_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called__If_Id_Is_Invalid(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns(GetTestToDoLists()[0]);
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Edit(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.GetByIdAsync(It.IsAny<int>()), Times.Exactly(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Edit_Post_Returns_RedirectToActionResult(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.Update(GetTestToDoLists()[0]));
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Edit(id, GetTestToDoListModels()[0]);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.Update(It.IsAny<ToDoList>()), Times.Exactly(1));
        }

        [Test]
        [TestCaseSource("IdAndToDoListModelTestCases")]
        public void Edit_Post_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called__If_Id_Or_Model_Is_Invalid((int,ToDoListModel) idAndToDoListModel)
        {
            // Arrange
            var id = idAndToDoListModel.Item1;
            var toDoListModel = idAndToDoListModel.Item2;
            mockRepo.Setup(repo => repo.ToDoLists.Update(It.IsAny<ToDoList>()));
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Edit(id, toDoListModel);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.GetByIdAsync(It.IsAny<int>()), Times.Exactly(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Delete_Returns_RedirectToActionResult(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns(GetTestToDoLists()[0]);
            mockRepo.Setup(repo => repo.ToDo.GetTodosForToDoListWithId(It.IsAny<int>()).Result)
                    .Returns(GetTestToDos());
            mockRepo.Setup(repo => repo.ToDoLists.Remove(It.IsAny<ToDoList>()));
            mockRepo.Setup(repo => repo.ToDo.RemoveRange(It.IsAny<IEnumerable<ToDo>>()));
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Delete(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.GetByIdAsync(It.IsAny<int>()), Times.Exactly(1));
            mockRepo.Verify(x => x.ToDo.GetTodosForToDoListWithId(It.IsAny<int>()), Times.Exactly(1));
            mockRepo.Verify(x => x.ToDoLists.Remove(It.IsAny<ToDoList>()), Times.Exactly(1));
            mockRepo.Verify(x => x.ToDo.RemoveRange(It.IsAny<IEnumerable<ToDo>>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Delete_Returns_RedirectToActionResult_And_Repository_TwoOutOfFour_Methods_Are_Called__If_Model_With_Id_Doesnt_Exists(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns((ToDoList)null);
            mockRepo.Setup(repo => repo.ToDo.GetTodosForToDoListWithId(It.IsAny<int>()).Result)
                    .Returns((List<ToDo>)null);
            mockRepo.Setup(repo => repo.ToDoLists.Remove(It.IsAny<ToDoList>()));
            mockRepo.Setup(repo => repo.ToDo.RemoveRange(It.IsAny<IEnumerable<ToDo>>()));
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Delete(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.GetByIdAsync(It.IsAny<int>()), Times.Exactly(1));
            mockRepo.Verify(x => x.ToDo.GetTodosForToDoListWithId(It.IsAny<int>()), Times.Exactly(1));
            //Remove methods not called
            mockRepo.Verify(x => x.ToDoLists.Remove(It.IsAny<ToDoList>()), Times.Exactly(0));
            mockRepo.Verify(x => x.ToDo.RemoveRange(It.IsAny<IEnumerable<ToDo>>()), Times.Exactly(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Copy_Returns_ViewResult(int id)
        {
            // Arrange
            var listWithTodos = GetTestToDoLists()[0];
            listWithTodos.ToDos = GetTestToDos();
            mockRepo.Setup(repo => repo.ToDoLists.GetToDoListWithToDosAsync(It.IsAny<int>()).Result)
                    .Returns(listWithTodos);
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Copy(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            mockRepo.Verify(x => x.ToDoLists.GetToDoListWithToDosAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Copy_Returns_RedirectToActionResult_If_Id_Is_Invalid_Repository_Method_Is_Not_Called(int id)
        {
            // Arrange
            var listWithTodos = GetTestToDoLists()[0];
            listWithTodos.ToDos = GetTestToDos();
            mockRepo.Setup(repo => repo.ToDoLists.GetToDoListWithToDosAsync(It.IsAny<int>()).Result)
                    .Returns(listWithTodos);
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Copy(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.GetToDoListWithToDosAsync(It.IsAny<int>()), Times.Exactly(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Copy_Returns_RedirectToActionResult_If_Model_With_Id_Was_Not_Found_Repository_Method_Is_Called(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDoLists.GetToDoListWithToDosAsync(It.IsAny<int>()).Result)
                    .Returns((ToDoList)null);
            var controller = new ToDoListController(mockListLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Copy(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDoLists.GetToDoListWithToDosAsync(It.IsAny<int>()), Times.Exactly(1));
        }

#pragma warning disable S1144 // Unused private types or members should be removed
        private static IEnumerable<(int,ToDoListModel)> IdAndToDoListModelTestCases
        {
            get
            {
                yield return (1, null);
                yield return (0, TestToDoListModel);
            }
        }
#pragma warning restore S1144 // Unused private types or members should be removed

        private static ToDoListModel TestToDoListModel => new ToDoListModel()
        {
            Id = 1,
            Title = "Test TodoList #1",
            CreationDate = System.DateTime.Today,
            IsVisible = true,
        };

        private List<ToDoList> GetTestToDoLists()
        {
            return new List<ToDoList>()
            {
                new ToDoList()
                {
                    Id = 1,
                    Title = "Test TodoList #1",
                    CreationDate = System.DateTime.Today,
                    IsVisible = true,
                },
                new ToDoList()
                {
                    Id = 2,
                    Title = "Test TodoList #2",
                    CreationDate = System.DateTime.Today,
                    IsVisible = false,
                },
                new ToDoList()
                {
                    Id = 3,
                    Title = "Test TodoList #3",
                    CreationDate = System.DateTime.Today,
                    IsVisible = true,
                }
            };
        }

        private List<ToDoListModel> GetTestToDoListModels()
        {
            return new List<ToDoListModel>()
            {
                new ToDoListModel()
                {
                    Id = 1,
                    Title = "Test TodoList #1",
                    CreationDate = System.DateTime.Today,
                    IsVisible = true,
                },
                new ToDoListModel()
                {
                    Id = 2,
                    Title = "Test TodoList #2",
                    CreationDate = System.DateTime.Today,
                    IsVisible = false,
                },
                new ToDoListModel()
                {
                    Id = 3,
                    Title = "Test TodoList #3",
                    CreationDate = System.DateTime.Today,
                    IsVisible = true,
                }
            };
        }

        private List<ToDo> GetTestToDos()
        {
            return new List<ToDo>()
            {
                new ToDo()
                {
                    Id = 1,
                    Title = "Test Todo #1",
                    Description = "Test Todo #1",
                    CreationDate = System.DateTime.Today,
                    Status = Domain.Enums.ToDoStatus.NotStarted,
                },
                new ToDo()
                {
                    Id = 2,
                    Title = "Test Todo #2",
                    Description = "Test Todo #2",
                    CreationDate = System.DateTime.Today,
                    Status = Domain.Enums.ToDoStatus.NotStarted,
                },
                new ToDo()
                {
                    Id = 3,
                    Title = "Test Todo #3",
                    Description = "Test Todo #3",
                    CreationDate = System.DateTime.Today,
                    Status = Domain.Enums.ToDoStatus.NotStarted,
                },
            };
        }
    }
}