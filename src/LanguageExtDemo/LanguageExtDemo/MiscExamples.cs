using LanguageExt;
using System;
using System.Collections.Generic;
using static LanguageExt.Prelude;

namespace LanguageExtDemo
{
    class MiscExamples
    {
        Unit MapExample()
        {
            Map<int, string> dict = Map<int, string>().Add(1, "str");
            Option<string> elementOpt = dict.Find(1);
            var element = elementOpt.IfNone("def");
            return unit;
        }

        Unit SeqExample()
        {
            Seq<int> seq = Seq(1, 2, 3);
            Option<int> head = seq.HeadOrNone();
            IEnumerable<int> tail = seq.Tail();

            int patterns = seq.Match(
                    One: identity,
                    More: (head, tail) => head * tail.Sum(),
                    Empty: () => 0);

            return unit;
        }

        Unit UnifiedContainersExtensionsExample()
        {
            string guidString = string.Empty;
            Option<Guid> guid = parseGuid(guidString);
            Option<Guid> nonDefaultGuid = guid.Filter(notDefault);
            Either<string, Guid> eitherGuidOrError = guid.ToEither("Not a guid");

            bool containsParticularGuid = eitherGuidOrError.Exists(guid => guid == Guid.Empty || guid == Guid.NewGuid());

            bool equals = eitherGuidOrError == Guid.Empty;

            return eitherGuidOrError.BiIter(
                Right: guid => Console.WriteLine(guid),
                Left: error => Console.WriteLine("Error" + error));
        }

        Unit TryExample()
        {
            int x = 0;
            Try<int> resultOrExeption = Try(() => 4 / x);
            Either<Exception, int> resultOrLeft = resultOrExeption.ToEither();

            return unit;
        }
    }
}
