namespace Fiap.TechChallenge.Domain.Response;


public class DefaultResponse<T>
{
    public DefaultResponse()
    {
    }

    public string? Message { get; set; }
    public T? Data { get; set; }
}
