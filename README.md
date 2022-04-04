This small framework is designed as a test task.

Currently it contains two tests for GetPetById endpoint of Swagger petstore service (https://petstore.swagger.io/).

In the process of development the following tools were used: 

Unit testing framework: NUnit, 
BDD Framework: SpecFlow, 
Test cases definition language: Gherkin, 
Logger: Serilog, 
Reporting framework: Allure, 


An example of Allure report is contained in Allure report example file 

To run an Allure report:
- run the tests
- proceed to \bin\Debug\net5.0
- from this folder run in command line 
    "allure generate allure-results --clean -o allure-report" 
  to  create the report and
    "allure open <path to allure report folder>" 
  to open it in browser.
