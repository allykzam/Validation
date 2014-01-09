This library serves the purpose of making it easier to validate parameters in a quick, readable way. It came about after I found [Rick Brewster's article][1] on using fluent programming to validate parameters, and is heavily based on the work he did. Before finding his article, I had been using a small class of methods inspired by the [Ensure class][2] in GitHub's octokit.net project. Rick's article provides good examples of the improvement that this syntax provides, but here's a quick mockup of the basic stuff I'm doing with it:


```C#
public static void someMethod(String a, int b, Object c, Object d)
{
    Validate.Begin()
        .IsNumeric(value: a, name: "a")
        .IsWithinRange(value: b, name: "b", min: 5, max: 50)
        .IsNotNull(value: c, name: "c")
        .ValidateWhen(b < 10).IsNotNull(value: d, name: "d")
        .Check()

    ...
}
```

This set of validations checks that the value of the string `a` must be a numeric string, `b` must be a whole number in the range of 5..50, `c` cannot be null, and if `b` is less than 10, then `d` cannot be null either. If any of these checks fail, the call to `.Check()` throws an `AggregateException` containing exceptions indicating which checks failed. This means that not only do we get readable code, but you also get all of the failures at once, rather than just hitting the first check and stopping execution there. However, if only one check fails, the call to `.Check()` will just throw an exception for that single failure. Exception types that each check contributes can be seen either in the source code, or by browsing each check's information in the Object Browser.

As seen with the check for `d` in the above example, the check methods take an optional `runIf` parameter which defaults to `true` and indicates whether or not the check needs to run. This may seem repetitive for general use (why would I need a flag to tell it whether or not to validate if I'm calling it to validate?), but the extra ability it provides is that you can conditionally validate values. Say you have a structure named `Temperature`, and you want the constructor to validate that the input is within a specific range. By using `runIf`, you can provide two different ranges depending on whether the input is in degrees C or degrees F. There may be better examples, but obviously if you do not need this functionality, don't use it.

If any failures occur, they set an internal flag which is checked when the validation code's destructor is called. If the values haven't been checked via `.Check()` (or `.ObserveExceptions()`), the destructor will throw the `AggregateException`. This means you will need to call `.Check()` whenever this validation code is used, or else you'll get a random exception later when the GC kicks in.

As an additional help, if you've imported the `Validation` namespace, there are two extension methods available, `AllMessages` and `AllExceptionTypes` which extend the `AggregateException` class, returning a concatenated string of the internal exception messages and types respectively, which should help you out depending on where you're logging your exceptions to. Obviously if the format I've chosen doesn't work for you, take a look at the code I wrote and feel free to come up with your own extensions with the formatting you'd prefer.

This library is available under version 2.1 of the GNU Lesser General Public License. The full license is available in the LICENSE file, and additional information about contributors will be added to a CONTRIBUTORS file at a time when it is necessary.

[1]: http://blog.getpaint.net/2008/12/06/a-fluent-approach-to-c-parameter-validation/
[2]: https://github.com/octokit/octokit.net/blob/master/Octokit/Helpers/Ensure.cs
