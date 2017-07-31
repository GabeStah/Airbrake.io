# Is Your Reputation Suffering from Unforced Production Errors?

Most teams and tech companies have been there -- in spite of everyone's best efforts, something goes unexpectedly wrong in production and things go from bad to worse in an instant.  No matter how many users are impacted, or how significant the financial loss may be, allowing unforced production errors is a major hindrance to you and your reputation.

`Unforced errors` are a term most readily found in tennis, whereby a player makes an error that is completely due to his or her failure, resulting in a point for the opponent.  In the realm of development, an unforced error is an error that occurs due to inexperience, poor planning, and/or improper development practices.  Such "errors" can be most anything for which you and your team are directly (or indirectly) at fault, such as sending out a promotional email that contains an invalid URL, displaying an incorrect UI component to a user, or even an actual application error that results in a failure for a customer (if not a crash of the entire system).

Making every effort to avoid unforced errors, particularly within a production environment, can be challenging, but doing so is critical for building up and maintaining the reputation of your team, your business, and your product.  In the rest of this article we'll explore a few techniques and suggestions, used by various business leaders around the world, that will help you and your team reduce your unforced error rates.

## Sit Down, Be Humble

Kendrick Lamar [offers this sage advice](https://www.youtube.com/watch?v=tvTRZJ-4EyI) in one of his latest hits, aptly titled "HUMBLE.", and in the realm of management and application development, humility is a quality that cannot be overlooked nor underappreciated.  While some degree of humility is relevant for everyone on the team, managers and executives, in particular, would be well-served to maintain more than a modicum of humility.  As author Jeffrey Krames puts it in his book, [_The Unforced Error_](http://jeffreykrames.com/books-by-jk/the-unforced-error/):

> "Humility [...] grows out of a healthy sense of self-confidence and [...] allows a person to admit to a failing, to a lack of knowledge, or to a mistake and then get on with doing whatever it takes to resolve the problem."

Arrogance is a dangerous quality that will inevitably lead to a faulty application, underappreciated workforce, and (eventually) an abundance of unforced errors.  Instead, everyone on the team should be given an appropriate sense of purpose and autonomy, to feel free to speak out and discuss potential problem areas.  A manager that can admit mistakes and listen to subordinates will find the outside input invaluable, as production releases will be more stable and less prone to unforced errors.

## Expect the Unexpected

Never be blindsided by something that "shouldn't happen."  For example, displaying an incorrect UI component to a user is not the fault of the user, even if said user is attempting to access a page or component that may be both unexpected and unintended.  _Your application should expect erratic user behavior._  A strong, robust piece of software should be designed and coded to accommodate such unpredictable actions.  

For example, you might consider ensuring that all possible execution paths in the codebase are covered by exception catches, so unexpected problems can redirect the user to a friendly error message.  The application should also be able to handle abnormal access attempts from users that may not have appropriate permissions, regardless of _why_ the user is trying to gain access.  Gracefully inform the user of the invalid access and redirect them to a component that is relevant to their attempt.

Such fail-safes also improve application security and reliability.  If your application is properly catching exceptions and handling unexpected access attempts, it's easy to include relevant logging and alert measures in the event that a user is trying to gain access for malicious purposes.

## Integrate Error Monitoring Software

Error monitoring software provides a constant pulse on the health and reliability of your application, whether its running in development or even out in the wilds of production.  All errors are quickly identified, examined, and can be fixed, without the need for convoluted user-generated reports or expensive and time-consuming quality assurance support.

The best error monitoring services allow you and your team to see the exact nature of every error, including a plethora of detailed metadata.  For example, when an error occurs using <a class="js-cta-utm" href="https://airbrake.io/?utm_source=sitepoint&amp;utm_medium=end-post&amp;utm_campaign=airbrake-unforced-error">Airbrake.io's error monitoring software</a>, Airbrake is able to report the error type, error message, parameters, the API method that generated the error, the detailed backtrace, the client machine data, the environment in which the error occurred, the file that caused the error, and much more.

Thus, not only does error monitoring software allow you to track and immediately identify exceptions when they occur, it also provides a substantial safety net, particularly during production releases.  While you'll still need some level of quality assurance, error monitoring services provide a bit of breathing room by promising to inform you of any unforeseen issues.

__META DESCRIPTION__

An assortment of tips and techniques for avoiding unforced productions errors, thereby helping to improve the reputation of you and your team.

---

__SOURCES__

- http://www.ideaarchitects.org/2016/04/avoid-unforced-errors.html
- https://www.amazon.com/Unforced-Error-Managers-Promoted-Eliminated/dp/B002YX0G0A