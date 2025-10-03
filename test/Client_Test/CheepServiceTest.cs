namespace Client_Test;

public class CheepServiceTest
{
    [Fact]
    public void getCheepsTest()
    {
        CheepService cs = new CheepService();
        Assert.Equal(2, cs.GetCheeps().Count());
    }

    [Fact]
    public void InsertUserAndMessageTest()
    {
        CheepService cs = new CheepService();
        cs.InsertUser("Test", "Test@itu.dk");
        cs.InsertMessage(cs.getIDfromEmail("Test@itu.dk"), "this is a test");
        Assert.NotEmpty(cs.GetCheepsFromAuthor("Test"));
    }
}