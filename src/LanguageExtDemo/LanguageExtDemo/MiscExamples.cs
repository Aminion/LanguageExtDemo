using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        IEnumerable<Task<int>> F() => throw new NotImplementedException();
        Option<IEnumerable<int>> F1() => throw new NotImplementedException();

        Unit TraversalExample()
        {

            IEnumerable<Task<string>> TaskOfEnumerable = F()
                .MapT(toString)
                .FilterT(s => s != string.Empty);


            Option<IEnumerable<string>> EnumerableOfEithers = F1()
                .MapT(toString)
                .FilterT(s => s != string.Empty);

            IEnumerable<Option<string>> EnumerableOfEithersFlip = EnumerableOfEithers.Sequence();

            Task<IEnumerable<string>> TaskOfEnumerableFlip = TaskOfEnumerable.Sequence();

            return unit;
        }


        Unit EnumerableHelpersExample()
        {
            var options = Enumerable.Empty<Option<int>>();
            Option<IEnumerable<int>> flippedOptions = options.Sequence();

            var validations = Enumerable.Empty<Validation<string,int>>();
            Validation<string, IEnumerable<int>> validationsOptions = validations.Sequence();

            var onlySomes = Enumerable.Range(1, 10).Choose(x => x % 2 == 0 ? Some(x) : None);

            return unit;
        }
    }
}
