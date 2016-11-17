Feature: Add license to trainee
	
@mytag
Scenario: License avialable
	Given Content Manager is open
	Then navigate to "User Management"
	Then chose a trainee
	When license is available 
		And user exists
	Then I should see "Successfull"

 Scenario:User doesn't exists
	Given Content Manager is open
	Then navigate to "User Management"
	Then chose a trainee
	When licesene is not available OR user doesn't exstis
	Then I should see error page "User doesn't exist"
	
Scenario: No license left
	Given Content Manager is open
	Then navigate to "User Management"
	Then chose a trainee
	When licesene is not available OR no license left
	Then I should see error page "No license left"