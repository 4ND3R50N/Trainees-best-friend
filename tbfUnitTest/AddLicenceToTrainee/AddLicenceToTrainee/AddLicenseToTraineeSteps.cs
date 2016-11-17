using System;
using TechTalk.SpecFlow;

namespace UnitTestProject1
{
    [Binding]
    public class AddLicenseToTraineeSteps
    {
        [Given(@"Content Manager is open")]
        public void GivenContentManagerIsOpen()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"license is available")]
        public void WhenLicenseIsAvailable()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"user exists")]
        public void WhenUserExists()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"licesene is not available OR user doesn't exstis")]
        public void WhenLiceseneIsNotAvailableORUserDoesnTExstis()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"licesene is not available OR no license left")]
        public void WhenLiceseneIsNotAvailableORNoLicenseLeft()
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
