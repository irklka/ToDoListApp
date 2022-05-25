using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ToDoListApp.Client.Controllers;
using ToDoListApp.Client.Models;
using ToDoListApp.Domain.Interfaces;
using ToDoListApp.Domain.Models;

namespace ToDoListApp.Tests
{
    public class ToDoControllerTests
    {
        private Mock<IUnitOfWork> mockRepo;
        private Mock<ILogger<ToDoController>> mockTodoLogger;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IUnitOfWork>();
            mockTodoLogger = new Mock<ILogger<ToDoController>>();
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Details_Returns_ViewResult_If_Model_Is_Returned(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns(GetTestToDos()[0]);
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Details(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            mockRepo.Verify(x => x.ToDo.GetByIdAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Details_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called_If_Id_Passed_Is_Invalid(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns(GetTestToDos()[0]);
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Details(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDo.GetByIdAsync(It.IsAny<int>()), Times.Exactly(0));
        }

        [Test]
        public void Details_Returns_RedirectToActionResult_And_Repository_Method_Is_Called_If_Model_Doesnt_exist()
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns((ToDo)null);
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Details(1).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDo.GetByIdAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Create_Returns_ViewResult_If_Id_Is_Valid(int listId)
        {
            // Arrange
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Create(listId);

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Create_Returns_RedirectToActionResult_If_Id_Is_Invalid(int listId)
        {
            // Arrange
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Create(listId);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public void Create_Post_Returns_RedirectToActionResult()
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.Add(It.IsAny<ToDo>()));
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Create(GetTestToDoModels()[0]).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDo.Add(It.IsAny<ToDo>()), Times.Exactly(1));
        }

        [Test]
        public void Create_Post_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called_If_Model_Is_Null_Or_Invalid()
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.Add(It.IsAny<ToDo>()));
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Create(null).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDo.Add(It.IsAny<ToDo>()), Times.Exactly(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Edit_Returns_ViewResult_If_Model_Is_Returned(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns(GetTestToDos()[0]);
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Edit(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
            mockRepo.Verify(x => x.ToDo.GetByIdAsync(It.IsAny<int>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void Edit_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called__If_Id_Is_Invalid(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns(GetTestToDos()[0]);
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Edit(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDo.GetByIdAsync(It.IsAny<int>()), Times.Exactly(0));
        }

        [Test]
        public void Edit_Post_Returns_RedirectToActionResult()
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.Update(It.IsAny<ToDo>()));
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Edit(1, GetTestToDoModels()[0]);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDo.Update(It.IsAny<ToDo>()), Times.Exactly(1));
        }

        [Test]
        [TestCaseSource("IdAndToDoModelTestCases")]
        public void Edit_Post_Returns_RedirectToActionResult_And_Repository_Method_Is_Not_Called__If_Id_Or_Model_Is_Invalid((int, ToDoModel) idAndToDoModel)
        {
            // Arrange
            var id = idAndToDoModel.Item1;
            var toDoModel = idAndToDoModel.Item2;
            mockRepo.Setup(repo => repo.ToDo.Update(It.IsAny<ToDo>()));
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Edit(id, toDoModel);

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDo.GetByIdAsync(It.IsAny<int>()), Times.Exactly(0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Delete_Returns_RedirectToActionResult(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns(GetTestToDos()[0]);
            mockRepo.Setup(repo => repo.ToDo.Remove(It.IsAny<ToDo>()));
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Delete(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDo.GetByIdAsync(It.IsAny<int>()), Times.Exactly(1));
            mockRepo.Verify(x => x.ToDo.Remove(It.IsAny<ToDo>()), Times.Exactly(1));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void Delete_Returns_RedirectToActionResult_And_Repository_OneOutOfTwo_Methods_Are_Called__If_Model_With_Id_Doesnt_Exists(int id)
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.GetByIdAsync(It.IsAny<int>()).Result)
                    .Returns((ToDo)null);
            mockRepo.Setup(repo => repo.ToDo.Remove(It.IsAny<ToDo>()));
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.Delete(id).Result;

            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            mockRepo.Verify(x => x.ToDo.GetByIdAsync(It.IsAny<int>()), Times.Exactly(1));
            mockRepo.Verify(x => x.ToDo.Remove(It.IsAny<ToDo>()), Times.Exactly(0));
        }

        [Test]
        public void ShowDueToday_Returns_ViewResult()
        {
            // Arrange
            mockRepo.Setup(repo => repo.ToDo.FindAsync(It.IsAny<Expression<Func<ToDo,bool>>>()).Result)
                    .Returns(GetTestToDos());
            var controller = new ToDoController(mockTodoLogger.Object, mockRepo.Object);

            // Act
            var result = controller.ShowDueToday().Result;

            // Assert
            Assert.That(result, Is.TypeOf<ViewResult>());
        }

#pragma warning disable S1144 // Unused private types or members should be removed
        private static IEnumerable<(int, ToDoModel)> IdAndToDoModelTestCases
        {
            get
            {
                yield return (1, null);
                yield return (0, TestToDoModel);
            }
        }
#pragma warning restore S1144 // Unused private types or members should be removed

        private static ToDoModel TestToDoModel => new ToDoModel()
        {
            Id = 1,
            Title = "Test Todo #1",
            Description = "Test Todo #1",
            CreationDate = System.DateTime.Today,
            Status = Domain.Enums.ToDoStatus.NotStarted,
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
        private List<ToDoModel> GetTestToDoModels()
        {
            return new List<ToDoModel>()
            {
                new ToDoModel()
                {
                    Id = 1,
                    Title = "Test Todo #1",
                    Description = "Test Todo #1",
                    CreationDate = System.DateTime.Today,
                    Status = Domain.Enums.ToDoStatus.NotStarted,
                },
                new ToDoModel()
                {
                    Id = 2,
                    Title = "Test Todo #2",
                    Description = "Test Todo #2",
                    CreationDate = System.DateTime.Today,
                    Status = Domain.Enums.ToDoStatus.NotStarted,
                },
                new ToDoModel()
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
