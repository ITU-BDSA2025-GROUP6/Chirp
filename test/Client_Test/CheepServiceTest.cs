// using Chirp.Infrastructure;
//
// namespace Client_Test;
//
// public class CheepServiceTest
// {
//     [Fact]
//     public void getCheepsTest()
//     {
//         CheepService cs = new CheepService();
//         Assert.True(cs.GetCheeps().Count() >= 1);
//     }
//
//     [Fact]
//     public void InsertUserAndMessageTest()
//     {
//         CheepService cs = new CheepService();
//         cs.InsertUser("Test", "Test@itu.dk");
//         cs.InsertMessage(cs.getIDfromEmail("Test@itu.dk"), "this is a test");
//         Assert.NotEmpty(cs.GetCheepsFromAuthor("Test"));
//     }
// }