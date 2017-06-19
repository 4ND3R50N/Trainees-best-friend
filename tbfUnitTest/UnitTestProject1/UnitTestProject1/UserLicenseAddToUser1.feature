Feature: Workout license
	
@mytag
Scenario: Add license to trainee
	Given Content Manager is open
	Then navigate to "User Management"
	Then chose a trainee
	When license is available and user exists
	Then I should see "Successfull"
	When licesene is not available or user doesn't exstis
	Then I should see error page "User doesn't exist"
	When licesene is not available or no license left
	Then I should see error page "No license left"
	Then I should see error page "Miau2344"