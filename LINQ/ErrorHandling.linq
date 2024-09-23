<Query Kind="Program" />

void Main()
{
	try
	{
		AggregateExceptionTest("James", "");
	}
	#region catch all exceptions
	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();
		}
	}
	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	#endregion
}

public Exception GetInnerException(System.Exception ex)
{
	while (ex.InnerException != null)
		ex = ex.InnerException;
	return ex;
}

public void AggregateExceptionTest(string firstName, string lastName)
{
	#region Business Logic and Parametter Exception
	//  create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();

	//	Business Rules
	//	These are processing rules that need to be satisfied
	//		for valid data
	//		rule: first name cannot be empty
	//		rule: last name cannot be empty

	//  parameter validation	
	if (string.IsNullOrWhiteSpace(firstName))
	{
		errorList.Add(new Exception("First name is required and cannot be empty"));
	}

	if (string.IsNullOrWhiteSpace(lastName))
	{
		errorList.Add(new Exception("Last name is required and cannot be empty"));
	}
	#endregion
	
	/*
		actual code for method
	*/
	
	if (errorList.Count() > 0)
	{
		//  throw the list of business processing error(s)
		throw new AggregateException("Unable to proceed! Check concerns", errorList);
	}
}

