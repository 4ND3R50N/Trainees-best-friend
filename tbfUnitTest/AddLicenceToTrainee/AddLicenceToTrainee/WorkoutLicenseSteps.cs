using System;
using TechTalk.SpecFlow;

namespace UnitTestProject1
{
    [Binding]
    public class WorkoutLicenseSteps
    {
        [Given(@"Content Manager is open")]
        public void GivenContentManagerIsOpen()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"license is available and user exists")]
        public void WhenLicenseIsAvailableAndUserExists()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"licesene is not available or user doesn't exstis")]
        public void WhenLiceseneIsNotAvailableOrUserDoesnTExstis()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"licesene is not available or no license left")]
        public void WhenLiceseneIsNotAvailableOrNoLicenseLeft()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"navigate to ""(.*)""")]
        public void ThenNavigateTo(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"chose a trainee")]
        public void ThenChoseATrainee()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see ""(.*)""")]
        public void ThenIShouldSee(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see error page ""(.*)""")]
        public void ThenIShouldSeeErrorPage(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
