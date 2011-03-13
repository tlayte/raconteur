﻿Feature: Table Types
	In order top make the Step implementation easier
	There are multiple types of Tables

Scenario: Single column Table becomes an array Arg

	Given the Feature contains
	"
		Scenario: Scenario Name
			Given some values:
			|0|
			|1|
	"

	The Runner should contain
	"
		[TestMethod]
		public void ScenarioName()
		{
			Given_some_values_
			(
				new[] {0, 1}
			);
		}
	"

@wip
Scenario: Object Table with single row becomes an object Arg

	Given the Feature contains
	"
		Scenario: Login User
			Given the User:
			[user name|password]
			|neo	  |53cr3t  |
	"
	And the Steps are "TableTypesSteps"
/*  
	containing a matching Step with an object Arg
	that contain matching properties to Header column
	
	public Given_the_User(User User)
	{
	}
*/
	The Runner should contain
	"
		[TestMethod]
		public void LoginUser()
		{
			Given_the_User
			( 
				new User
				{
					UserName = ""neo"",
					Password = ""53cr3t"",
				}
			);
		}
	"

Scenario: Object Table with multiple rows becomes an object[] Arg
