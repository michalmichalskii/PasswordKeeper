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
        public void CanAddItem()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var userDataService = new UserDataService();

            //Act
            userDataService.AddItem(userDataModel);
            userDataService.AddItem(userDataModel);
            userDataService.AddItem(userDataModel);

            var countOfItems = userDataService.GetAllItems();

            //Assert
            countOfItems.Should().NotBeNullOrEmpty();
            countOfItems.Count.Should().Be(3);
        }

        [Fact]
        public void CanDeleteItems()
        {
            //Arrange
            var userDataModel = new User(1, "wp.pl", "mich@wp.pl", "Password1");
            var userDataService = new UserDataService();

            //Act
            userDataService.AddItem(userDataModel);
            userDataService.DeleteItemById(userDataModel.Id);
            var countOfItems = userDataService.GetAllItems();

            userDataService.GenerateRandomPassword();

            //Assert
            countOfItems.Should().BeEmpty();
            countOfItems.Count.Should().Be(0);
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