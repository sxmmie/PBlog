using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogger.Helpers
{
    public static class PageHelper
    {
        public static IEnumerable<int> PageNumbers(int pageNumber, int pageCount)
        {
            if (pageCount <= 5)
            {
                for (var i = 1; i <= pageCount; i++)
                {
                    yield return i;
                }
            }
            else
            {

                int midPoint = pageNumber < 3 ? 3 : pageNumber > pageCount - 2 ? pageCount - 2 : pageNumber;

                var lowerBound = midPoint - 2;
                var upperBound = midPoint + 2;

                if (lowerBound != 1)
                {
                    yield return 1;
                    if (lowerBound - 1 > 1)
                    {
                        yield return 1;
                    }
                }

                for (var i = midPoint - 2; i <= midPoint + 2; i++)
                {
                    yield return i;
                }

                if (upperBound != pageCount)
                {
                    if (pageCount - upperBound > 1)
                    {
                        yield return -1;
                    }

                    yield return pageCount;
                }
            }
        }
    }
}
