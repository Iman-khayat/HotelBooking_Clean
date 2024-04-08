Feature: CreateBooking
  """
  The customer John Smith can create a booking from tomorrow and onwards only if there 
  is a free room for the entire specified period startdate-enddate.
  """

  //@mytag
  Scenario Outline: CreateBooking
   Given John Smith are at the create booking page having selected
   * <StartDateOffset>
   * <EndDateOffset>
   When John Smith press the Create button
   Then his booking is <created>

   Examples: 
    | StartDateOffset | EndDateOffset | Created         | # Comments                                      |
    | -1              | -1            | false           | # cannot book a room in the past                |
    | 0               | 0             | false           | # cannot book a room today                      |
    | 2               | 1             | false           | # StartDate must be earlier or equal to Enddate |
    | 3               | 3             | true            | # room available                                |
    | 3               | 4             | false           | # room not available on enddate                 |
    | 4               | 4             | false           | # room not available                            |
    | 18              | 18            | false           | # room not available                            |
    | 18              | 19            | false           | # room not available on startdate               |
    | 19              | 19            | true            | # room available                                |
