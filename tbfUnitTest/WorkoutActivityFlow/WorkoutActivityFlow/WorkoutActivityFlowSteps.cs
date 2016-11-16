using System;
using TechTalk.SpecFlow;

namespace WorkoutActivityFlow
{
    [Binding]
    public class WorkoutActivityFlowSteps
    {
        [Given(@"TBF App is open")]
        public void GivenTBFAppIsOpen()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Trainee is not loged in")]
        public void WhenTraineeIsNotLogedIn()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Trainee is loged in")]
        public void WhenTraineeIsLogedIn()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see signupPage")]
        public void ThenIShouldSeeSignupPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see roomPage")]
        public void ThenIShouldSeeRoomPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I navigate to a room")]
        public void ThenINavigateToARoom()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see levelPage")]
        public void ThenIShouldSeeLevelPage()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I navigate to a level")]
        public void ThenINavigateToALevel()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see Video and Description")]
        public void ThenIShouldSeeVideoAndDescription()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
