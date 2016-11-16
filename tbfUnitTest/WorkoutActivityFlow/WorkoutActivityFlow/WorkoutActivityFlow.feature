Feature: WorkoutActivityFlow
	
@mytag
Scenario: Login -> Join room -> Chose level -> View content
	Given TBF App is open
	When Trainee is not loged in
	Then I should see signupPage
	When Trainee is loged in 
	Then I should see roomPage
	Then I navigate to a room
	Then I should see levelPage
	Then I navigate to a level
	Then I should see Video and Description


