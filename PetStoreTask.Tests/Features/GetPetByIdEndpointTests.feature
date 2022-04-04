Feature: GetPetByIdEndpointTests
	Tests that verify GetPetById endpoint functionality

@deleteTestPet
Scenario: GetPetById endpoint returns correct data
	Given Test pet is posted to the database
	When GET request is sent
	Then Pet from API shall be equal to expected test pet

Scenario: GetPetById endpoint returns correct error message
	Given GET request is sent with incorrect '<id>'
	Then Correct '<expectedErrorMessage>' should be returned

	Examples:
		| id | expectedErrorMessage |
		| 0  | Pet not found        |