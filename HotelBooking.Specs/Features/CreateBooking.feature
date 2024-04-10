Feature: CreateBooking
The customer John Smith can create a booking from tomorrow and onwards only if there 
is a free room for the entire specified period startdate-enddate.

@CreateBookingValidInputs
Scenario Outline: CreateBooking valid inputs
    Given John Smith are at the create booking page having selected <StartDateOffset>
    And <EndDateOffset>
    When John Smith press the Create button
    Then his booking is <Created>

Examples: 
    | StartDateOffset | EndDateOffset | Created         | # Comments                                      |
    | 1               | 1             | true            | # room available                                |
    | 3               | 3             | true            | # room available                                |
    | 3               | 4             | false           | # room not available on enddate                 |
    | 4               | 4             | false           | # room not available                            |
    | 11              | 11            | false           | # room not available                            |
    | 18              | 18            | false           | # room not available                            |
    | 18              | 19            | false           | # room not available on startdate               |
    | 19              | 19            | true            | # room available                                |
    | 25              | 25            | true            | # room available                                |

# BVT: Boundary value testing: We are testing the boundary values of StartDate and EndDate, 
#      i.e. around dates which are possible and not possible to book
#      (Cannot book room today or in the past - see HotelBooking.Specs/Features/CreateBookingArgmentException,
#      Room is not available in period: StartDateOffset = 4 to EndDateOffset = 18)

# ECT: Equivalence class testing: Test valid inputs in the disjoint equivalence classes
#      (test near the limits of a class and in the middle of the class:
#       class 1: StartDateOffset = 1, EndDateOffset = 3, i.e. the period from tomorrow until 3 days ahead where can book and room is free
#       class 2: StartDateOffset = 4, EndDateOffset = 18, i.e. the period where room is not available
#       class 3: period after class 2 where room is available