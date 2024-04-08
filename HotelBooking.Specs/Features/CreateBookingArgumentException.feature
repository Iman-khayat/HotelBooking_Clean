Feature: CreateBookingArgumentException
The customer John Smith can create a booking from tomorrow and onwards only if there 
is a free room for the entire specified period startdate-enddate.

@CreateBookingArgumentException
Scenario Outline: CreateBooking throw ArgumentException
The customer John Smith can not create a booking in the past, today or having StartDate later than EndDate.

    Given John Smith have selected <StartDateOffset>
    And EndDateOffset <EndDateOffset>
    When John Smith press create
    Then an ArgumentException is thrown

Examples: 
    | StartDateOffset | EndDateOffset | Created         | # Comments                                      |
    | -1              | -1            | false           | # cannot book a room in the past                |
    | 0               | 1             | false           | # cannot book a room in the past (ECT: mixing valid and invalid inputs |
    | 0               | 0             | false           | # cannot book a room today                      |
    | 2               | 1             | false           | # StartDate must be earlier or equal to Enddate |

# BVT: boundary value testing
# ECT: equivalence class testing