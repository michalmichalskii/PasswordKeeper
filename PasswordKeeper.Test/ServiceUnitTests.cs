using FluentAssertions;
using Moq;
using PasswordKeeper.App.Abstarct;
using PasswordKeeper.App.Common;
using PasswordKeeper.App.Concrete;
using PasswordKeeper.App.Managers;
using PasswordKeeper.Domain.Entity;

namespace PasswordKeeper.Test
{
    public class ServiceUnitTests
    {
        [Fact]
        public void AddItem_ReturnsTheirId()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var userDataModel2 = new User(2, "wp.pl", "mich@wp.pl", "Password1");
            var userDataModel3 = new User(3, "wp.pl", "mich@wp.pl", "Password1");
            var userDataService = new UserDataService();

            //Act
            int userOneId = userDataService.AddItem(userDataModel);
            int userTwoId = userDataService.AddItem(userDataModel2);
            int userThreeId = userDataService.AddItem(userDataModel3);

            //Assert
            userOneId.Should().Be(1);
            userTwoId.Should().Be(2);
            userThreeId.Should().Be(3);
        }

        [Fact]
        public void GetItemByValidId_ReturnsItem()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var userDataService = new UserDataService();
            userDataService.AddItem(userDataModel);

            //Act
            var userOne = userDataService.GetItemById(1);

            //Assert
            userOne.Should().Be(userDataModel);
        }

        [Fact]
        public void DeleteItemByValidId_ReturnsTrue()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var userDataService = new UserDataService();
            userDataService.AddItem(userDataModel);

            //Act
            bool isDeleted = userDataService.DeleteItemById(userDataModel.Id);

            //Assert
            isDeleted.Should().Be(true);
        }

        [Fact]
        public void DeleteItemByInvalidId_ReturnsFalse()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var userDataService = new UserDataService();
            userDataService.AddItem(userDataModel);

            //Act
            bool isDeleted = userDataService.DeleteItemById(2);

            //Assert
            isDeleted.Should().Be(false);
        }

        [Fact]
        public void DeleteValidItem_ReturnsTrue()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var userDataService = new UserDataService();
            userDataService.AddItem(userDataModel);

            //Act
            bool isDeleted = userDataService.DeleteItem(userDataModel);

            //Assert
            isDeleted.Should().Be(true);
        }

        [Fact]
        public void DeleteInvalidItem_ReturnsFalse()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var userDataService = new UserDataService();

            //Act
            bool isDeleted = userDataService.DeleteItem(userDataModel);

            //Assert
            isDeleted.Should().Be(false);
        }

        [Fact]
        public void GetLastId()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var userDataService = new UserDataService();
            userDataService.AddItem(userDataModel);

            //Act
            int lastId = userDataService.GetLastId();

            //Assert
            lastId.Should().Be(1);
        }

        [Fact]
        public void GetAllItems_ReturnsListOfItems()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var userDataService = new UserDataService();
            userDataService.AddItem(userDataModel);

            //Act
            var listOfItems = userDataService.GetAllItems();

            //Assert
            listOfItems.Should().BeOfType(typeof(List<User>));
            listOfItems.Should().HaveCount(1);
            listOfItems.Should().Contain(userDataModel);
        }

        [Fact]
        public void IsValidSiteAvailable_ReturnsTrue()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var webService = new WebService();

            //Act
            bool isAvaile = webService.CheckIsSiteAvailable(userDataModel.Site);

            //Assert
            isAvaile.Should().Be(true);
        }

        [Fact]
        public void IsInvalidSiteAvailable_ReturnsFalse()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var webService = new WebService();

            //Act
            bool isAvaile = webService.CheckIsSiteAvailable("ok");

            //Assert
            isAvaile.Should().Be(false);
        }

        [Fact]
        public void RandomPasswordCheck()
        {
            //Arrange
            var userDataService = new UserDataService();
            char[] alphabet = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] numbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] specialChars = new[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '[', '{', ']', '}', ';', ':', '\'', '\"', ',', '.', '<', '>', '/', '?', '\\' };

            //Act
            string pass = userDataService.GenerateRandomPassword();

            //Assert
            pass.Length.Should().BeGreaterThan(15);
            pass.Should().ContainAny(alphabet.Select(c => c.ToString()));
            pass.Should().ContainAny(alphabet.Select(c => c.ToString().ToUpper()));
            pass.Should().ContainAny(numbers.Select(c => c.ToString()));
            pass.Should().ContainAny(specialChars.Select(c => c.ToString()));

        }
    }
}