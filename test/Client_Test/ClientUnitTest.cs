namespace Client_Test;

using Xunit;

public class ClientUnitTest
{
	[Fact]

	public void Timestamp_Test()
	{
		//Arrange
		var TestUnixTimpestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		var testCheep = new Cheep() { };

		//Act
		//Assert
	}
    public void Username_Test()
    {
		//Arange
		
		//Act
		//Assert
        
    }
	
	public void Unix_Timestamp_Test()
	{
		//Arrange
		var testUnixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		var testCheep = new Cheep()
		{
			user_name = "test",
			user_message = "test",
			unixTimeStamp = testUnixTimestamp
		};
		
		
		//Act
		
			
		
		//Assert
	}

	public void Message_Test()
	{
		//Arrange
		//Act
		//Assert
	}

}