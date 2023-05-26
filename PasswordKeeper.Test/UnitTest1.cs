using Moq;
using PasswordKeeper.App.Abstarct;
using PasswordKeeper.App.Concrete;
using PasswordKeeper.App.Managers;
using PasswordKeeper.Domain.Entity;

namespace PasswordKeeper.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            UserDataModel userDataModel = new UserDataModel(1,"wp.pl","mich@op.pl","Password1");
            var mock = new Mock<IService<UserDataModel>>();
            mock.Setup(s => s.GetItemById(1)).Returns(userDataModel);

            var manager = new UserDataManager(new MenuActionService(), mock.Object);
            //Act

            var returnedUserDataModel = manager.GetUserById(userDataModel.Id);
            //Assert

            Assert.Equal(userDataModel, returnedUserDataModel);
        }
    }
}