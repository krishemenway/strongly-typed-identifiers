using Microsoft.AspNetCore.Mvc;
using StronglyTyped.IntIds;

namespace ExampleService
{
	public class TestController : ControllerBase
	{
		[Route("Test"), HttpPost]
		public ActionResult<TestResponse> Something([FromBody] TestRequest request)
		{
			return new TestResponse
			{
				RequestId = request.RequestId,
				GeneratedId = Id<Test>.NewId(),
			};
		}

		public struct Test
		{
		}

		public class TestRequest
		{
			public Id<Test> RequestId { get; set; }
		}

		public class TestResponse
		{
			public Id<Test>? RequestId { get; set; }
			public Id<Test>? GeneratedId { get; set; }
		}
	}
}
