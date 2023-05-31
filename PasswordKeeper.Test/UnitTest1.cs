using FluentAssertions;
using Moq;
using PasswordKeeper.App.Abstarct;
using PasswordKeeper.App.Common;
using PasswordKeeper.App.Concrete;
using PasswordKeeper.App.Managers;
using PasswordKeeper.Domain.Entity;

namespace PasswordKeeper.Test
{
    public class UnitTest1
    {
        [Fact]
        public void CanGetUserWithProperId()
        {
            //Arrange
            User userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var mock = new Mock<IService<User>>();
            mock.Setup(s => s.GetItemById(1)).Returns(userDataModel);

            var manager = new UserDataManager(new MenuActionService(), mock.Object);
            //Act

            var returnedUserDataModel = manager.GetUserById(userDataModel.Id);

            //Assert
            returnedUserDataModel.Should().BeOfType(typeof(User));
            returnedUserDataModel.Should().NotBeNull();
            returnedUserDataModel.Should().BeSameAs(userDataModel);
        }

        [Fact]
        public void CanGetPasswordsList()
        {
            //Arrange
            User userDataModel1 = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            User userDataModel2 = new User(1, "op.pl", "mich@op.pl", "Password2");
            User userDataModel3 = new User(1, "polska.pl", "mich@polska.pl", "Password3");
            var mock = new Mock<IService<User>>();
            mock.Setup(s => s.Items).Returns(new List<User> { userDataModel1,userDataModel2,userDataModel3 });

            var manager = new UserDataManager(new MenuActionService(), mock.Object);

            //Act
            var returnedList = manager.GetPasswordsList();

            //Assert
            returnedList.Should().BeOfType(typeof(List<User>));
            returnedList.Should().NotBeNull();
            returnedList.Should().HaveCount(3);
        }

        [Fact]
        public void CanAddUser()
        {
            // Arrange
            var mockUserDataService = new Mock<IService<User>>();
            mockUserDataService.Setup(s => s.GetLastId()).Returns(10); // Ustawiamy wartoœæ zwracan¹ przez GetLastId()
            mockUserDataService.Setup(s => s.AddItem(It.IsAny<User>())); // Atrapujemy wywo³anie metody AddItem()

            var manager = new UserDataManager(new MenuActionService(), mockUserDataService.Object);

            // Act
            var result = manager.AddNewUserData();

            // Assert
            mockUserDataService.Verify(s => s.GetLastId(), Times.Once); // Sprawdzamy, czy GetLastId() zosta³o wywo³ane raz
            mockUserDataService.Verify(s => s.AddItem(It.IsAny<User>()), Times.Once); // Sprawdzamy, czy AddItem() zosta³o wywo³ane raz

            Assert.NotNull(result); // Sprawdzamy, czy zwrócony obiekt nie jest null

        }

        [Fact]
        public void CanDeleteUserDataModelWithProperId()
        {
            //Arrange
            User user = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            //IService<User> service = new BaseService<User>();
            var mock = new Mock<IService<User>>();
            mock.Setup(mock => mock.AddItem(user));

            var manager = new UserDataManager(new MenuActionService(), mock.Object);
            //var manager = new UserDataManager(new MenuActionService(),service);

            //Act
            var deletedResult = manager.RemoveUserById();

            //Assert
            deletedResult.Should().Be(user.Id);
        }

        [Fact]
        public void CanNotDeleteUserDataModelWithInvalidId()
        {
            //Arrange
            User userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var mock = new Mock<IService<User>>();
            mock.Setup(mock => mock.GetItemById(userDataModel.Id)).Returns(userDataModel);
            mock.Setup(mock => mock.DeleteItem(It.IsAny<User>()));

            var manager = new UserDataManager(new MenuActionService(), mock.Object);

            //Act
            var deletedResult = manager.RemoveUserById();

            //Assert
            deletedResult.Should().Be(0);
        }
    }
}