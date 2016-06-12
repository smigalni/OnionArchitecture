namespace OnionArchitectureExample
{
    public class Result
    {
        public Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;

        public static Result Ok(int number)
        {
            return new Result(true, string.Empty);
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result<T> Fail<T>(T value, string message)
        {
            return new Result<T>(value, false, message);
        }
    }

    public class Result<T> : Result
    {
        public Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            Value = value;
        }
        public T Value { get; }
    }
}