Feature: Login  Join room  Chose level  View content
	
@mytag
Scenario: Not log in
	Given TBF App is open
	When Trainee is not loged in
	Then I should see signupPage

Scenario: Log in
	Given TBF App is open
	When Trainee is loged in 
	Then I should see roomPage
	Then I navigate to a room
	Then I should see levelPage
	Then I navigate to a level
	Then I should see Video and Description