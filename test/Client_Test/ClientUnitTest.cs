// using Xunit.Abstractions;
// using Microsoft.Data.Sqlite;
//
// namespace Client_Test;
//
// using Xunit;
//
// public class ClientUnitTest
// {
// 	[Fact]
// 	public void Timestamp_Test()
// 	{
// 		//Arrange
// 		long unixTime = 1700000000;
// 		Cheep testCheep = new Cheep()
// 		{
// 			user_name = "Test", 
// 			user_message = "test",
// 			unixTimeStamp = unixTime
// 		};
//
// 		//Act
// 		var unix_result = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
// 		var cheep_unix_result = DateTimeOffset.FromUnixTimeMilliseconds(testCheep.unixTimeStamp);
// 		
// 		//Assert
// 		Assert.Equal(unix_result, cheep_unix_result);
// 	}
// 	
// 	[Fact]
// 	public void Timestamp_Now_Test()
// 	{
// 		//Arrange
// 		long unixTimeNow = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
// 		Cheep testCheep = new Cheep()
// 		{
// 			user_name = "Test", 
// 			user_message = "test",
// 			unixTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
// 		};
//
// 		//Act
// 		var unix_result = DateTimeOffset.FromUnixTimeMilliseconds(unixTimeNow);
// 		var cheep_unix_result = DateTimeOffset.FromUnixTimeMilliseconds(testCheep.unixTimeStamp);
// 		
// 		//Assert
// 		Assert.Equal(unix_result, cheep_unix_result);
// 	}
//
// 	[Fact]
// 	public void User_Readable_Timestamp_CEST_Test()
// 	{
// 		//Arrange
// 		Cheep cheep1 = new Cheep()
// 		{
// 			user_name = "sebastianhsvendsen",
// 			user_message = "brother i am testing this here too",
// 			unixTimeStamp = 1757510356
// 		};
// 		
// 		Cheep cheep2 = new  Cheep()
// 		{
// 			user_name = "andreasschioettmac",
// 			user_message = "this is a test",
// 			unixTimeStamp = 1757667210
// 		};
//
// 		Cheep cheep3 = new Cheep()
// 		{
// 			user_name = "ninacelinehansson",
// 			user_message = "single test",
// 			unixTimeStamp = 1757850545
//
// 		};
// 		
// 		//Act
// 		var when1 = DateTimeOffset.FromUnixTimeSeconds(cheep1.unixTimeStamp).AddHours(2);
// 		var when2 = DateTimeOffset.FromUnixTimeSeconds(cheep2.unixTimeStamp).AddHours(2);
// 		var when3 = DateTimeOffset.FromUnixTimeSeconds(cheep3.unixTimeStamp).AddHours(2);
// 		
// 		string str1 = when1.ToString("dd-MM-yyyy HH:mm");
// 		string str2 = when2.ToString("dd-MM-yyyy HH:mm");
// 		string str3 = when3.ToString("dd-MM-yyyy HH:mm");
//
// 		string test_str1 = "10-09-2025 15:19";
// 		string test_str2 = "12-09-2025 10:53";
// 		string test_str3 = "14-09-2025 13:49";
// 		
// 		//Assert
// 		Assert.Equal(str1, test_str1);
// 		Assert.Equal(str2, test_str2);
// 		Assert.Equal(str3, test_str3);
// 	}
// }