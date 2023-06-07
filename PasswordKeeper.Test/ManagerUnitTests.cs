using FluentAssertions;
using Moq;
using PasswordKeeper.App.Abstarct;
using PasswordKeeper.App.Common;
using PasswordKeeper.App.Concrete;
using PasswordKeeper.App.Managers;
using PasswordKeeper.Domain.Entity;

namespace PasswordKeeper.Test
{
    public class ManagerUnitTests
    {
        [Fact]
        public void CanGetUserWithProperId()
        {
            //Arrange
            User userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var mockUserDataService = new Mock<IService<User>>();
            mockUserDataService.Setup(s => s.GetItemById(1)).Returns(userDataModel);

            var manager = new UserDataManager(new MenuActionService(), mockUserDataService.Object, new JsonFileService(mockUserDataService.Object));
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
            var mockUserDataService = new Mock<IService<User>>();
            mockUserDataService.Setup(s => s.Items).Returns(new List<User> { userDataModel1,userDataModel2,userDataModel3 });

            var manager = new UserDataManager(new MenuActionService(), mockUserDataService.Object, new JsonFileService(mockUserDataService.Object));

            //Act
            var returnedList = manager.GetPasswordsList();

            //Assert
            returnedList.Should().BeOfType(typeof(List<User>));
            returnedList.Should().NotBeNull();
            returnedList.Should().HaveCount(3);
        }
    }
}