using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Typo3.Tests
{

    [TestFixture]
    public class AttemptTests
    {
        [TestCase]
        public void initial_values_are_set_correctly()
        {
            var attempt = new Attempt("foo");
            Assert.That(attempt.StartTime, Is.EqualTo(DateTime.Now).Within(new TimeSpan(0, 0, 1)));
            Assert.That(attempt.EndTime, Is.EqualTo(DateTime.Now).Within(new TimeSpan(0, 0, 1)));
            Assert.That(attempt.IsFinished, Is.False);
            Assert.That(attempt.IsGood, Is.True);
            Assert.That(attempt.OriginalText, Is.EqualTo("foo"));
        }

        [TestCase]
        public void completed_attempt_correctly_in_one_go()
        {
            var attempt = new Attempt("foo");
            attempt.AddText("foo");

            Assert.That(attempt.IsFinished, Is.True);
            Assert.That(attempt.IsGood, Is.True);
            Assert.That(attempt.EndTime, Is.EqualTo(DateTime.Now).Within(new TimeSpan(0, 0, 1)));
            Assert.That(attempt.StartTime, Is.EqualTo(DateTime.Now).Within(new TimeSpan(0, 0, 1)));
        }

        [TestCase]
        public void completed_attempt_correctly_in_several_goes()
        {
            var attempt = new Attempt("foo");

            // not finished but still good
            attempt.AddText("f");
            Assert.That(attempt.IsFinished, Is.False);
            Assert.That(attempt.IsGood, Is.True);

            // still not finished but still good
            attempt.AddText("o");
            Assert.That(attempt.IsFinished, Is.False);
            Assert.That(attempt.IsGood, Is.True);


            // will be finished at this point
            attempt.AddText("o");
            Assert.That(attempt.IsFinished, Is.True);
            Assert.That(attempt.IsGood, Is.True);
        }


        [TestCase]
        public void completed_bad_attempt_correctly_in_several_goes()
        {
            var attempt = new Attempt("fooBar");

            // not finished but still good
            attempt.AddText("f");
            Assert.That(attempt.IsFinished, Is.False);
            Assert.That(attempt.IsGood, Is.True);

            // finished and not good
            attempt.AddText("O");
            Assert.That(attempt.IsFinished, Is.True);
            Assert.That(attempt.IsGood, Is.False);
        }

        [TestCase]
        public void completed_bad_attempt_with_text_longer_than_original()
        {
            var attempt = new Attempt("foo");

            // not finished but still good
            attempt.AddText("fooBar");
            Assert.That(attempt.IsFinished, Is.True);
            Assert.That(attempt.IsGood, Is.False);
        }
    }
}
