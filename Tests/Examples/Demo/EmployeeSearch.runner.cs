using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Examples.Demo 
{
    [TestClass]
    public partial class EmployeeSearch 
    {
        
        [TestMethod]
        public void FindExistingEmployees()
        {         
            When_I_search_for_existing_Employees();        
            I_should_be_able_to_find_them();
        }
        
        [TestMethod]
        public void FindByFirstName()
        {         
            Given_an_Employee_named("Marco", "Polo");        
            When_I_search_for_Employees_with_first_name("Marco");        
            I_should_find_one_with_last_name("Polo");
        }

    }
}