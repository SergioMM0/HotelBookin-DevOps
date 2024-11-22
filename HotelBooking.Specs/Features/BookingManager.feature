Feature: Create Booking

# Equivalence Class 1, SD: B, BA: B
  Scenario Outline: Booking with start date <start_date> and end date <end_date> before occupied range
    Given the hotel has 1 rooms available
    And there is an occupied range of dates in the booking system from "5 days from now" to "10 days from now"
    And the booking start date is "<start_date>"
    And the booking end date is "<end_date>"
    When I try to create a booking
    Then the booking should be created successfully

    Examples:
      | start_date     | end_date                 |
      | tomorrow       | 2 days after the start date | 
      | 3 days from now | 1 day after the start date | 
      | 2 days from now | 1 day after the start date | 
      | 2 days from now | 4 days from now           | 


# Equivalence Class 2, SD: A, BA: A
  Scenario Outline: Booking with start date <start_date> and end date <end_date> after occupied range
    Given the hotel has 1 rooms available
    And there is an occupied range of dates in the booking system from "5 days from now" to "10 days from now"
    And the booking start date is "<start_date>"
    And the booking end date is "<end_date>"
    When I try to create a booking
    Then the booking should be created successfully

    Examples:
      | start_date        | end_date                 |
      | 11 days from now  | 15 days from now          |
      | 2 days before the end of time | 1 day before the end of time | 
      | 1 day after the occupied range | 1 day after the start date | 
      | 5 day after the occupied range | 1 day before the end of time | 

# Equivalence Class 3, Start Date: Available, End Date: Occupied
  Scenario Outline: Start date available, end date occupied
    Given the hotel has 1 rooms available
    And there is an occupied range of dates in the booking system from "<occupied_start_date>" to "<occupied_end_date>"
    And the booking start date is "<start_date>"
    And the booking end date is "<end_date>"
    When I try to create a booking
    Then the booking should fail to be created

  Examples:
    | occupied_start_date | occupied_end_date | start_date  | end_date        |
    | today               | 2 days from now   | tomorrow    | 3 days from now |
    | today               | 5 days from now   | tomorrow    | 4 days from now |
    | 2 days from now     | 5 days from now   | 3 days from now | 6 days from now |

# Klasse 4, Startdato før optaget periode, slutdato indenfor optaget periode
Scenario Outline: Start date before occupied, end date within occupied
    Given the hotel has 1 rooms available
    And there is an occupied range of dates in the booking system from "<occupied_start_date>" to "<occupied_end_date>"
    And the booking start date is "<start_date>"
    And the booking end date is "<end_date>"
    When I try to create a booking
    Then the booking should fail to be created

Examples:
    | occupied_start_date | occupied_end_date | start_date    | end_date          |
    | tomorrow           | 3 days from now   | today          | 2 days from now   |
    | today               | 4 days from now   | yesterday     | 2 days from now   |
    | 2 days from now     | 5 days from now   | tomorrow      | 4 days from now   |

# Klasse 5, Startdato indenfor optaget periode, slutdato efter optaget periode
Scenario Outline: Start date within occupied, end date after occupied
    Given the hotel has 1 rooms available
    And there is an occupied range of dates in the booking system from "<occupied_start_date>" to "<occupied_end_date>"
    And the booking start date is "<start_date>"
    And the booking end date is "<end_date>"
    When I try to create a booking
    Then the booking should fail to be created

Examples:
    | occupied_start_date | occupied_end_date | start_date    | end_date          |
    | today               | 2 days from now   | tomorrow      | 3 days from now   |
    | tomorrow           | 4 days from now   | 2 days from now | 5 days from now   |
    | 2 days from now     | 5 days from now   | 3 days from now | 6 days from now   |


# Equivalence Class 6, Start Date: Occupied, End Date: Available
  Scenario Outline: Start date occupied, end date available
    Given the hotel has 1 rooms available
    And there is an occupied range of dates in the booking system from "<occupied_start_date>" to "<occupied_end_date>"
    And the booking start date is "<start_date>"
    And the booking end date is "<end_date>"
    When I try to create a booking
    Then the booking should fail to be created

  Examples:
    | occupied_start_date | occupied_end_date | start_date  | end_date        |
    | tomorrow           | 3 days from now   | today        | 2 days from now |
    | today               | 4 days from now   | tomorrow    | 5 days from now |
    | 2 days from now     | 5 days from now   | 3 days from now | 6 days from now |

# Klasse 7, End date after "the end of time"
#Scenario Outline: End date after "the end of time"
#    And the booking start date is "<start_date>"
#    And the booking end date is "<end_date>"
#    When I try to create a booking
#    Then the booking should fail to be created  # Or handle it appropriately
#
#Examples:
#    | start_date    | end_date                      |
#    | today          | 2 days after the end of time |
#    | tomorrow      | 1 day after the end of time   |
#    | 2 days from now | the end of time               |

# Equivalence Class 8, Start Date: Before occupied, End Date: After Occupied
  Scenario Outline: Start date before occupied, end date after occupied
    Given the hotel has 1 rooms available
    And there is an occupied range of dates in the booking system from "<occupied_start_date>" to "<occupied_end_date>"
    And the booking start date is "<start_date>"
    And the booking end date is "<end_date>"
    When I try to create a booking
    Then the booking should fail to be created

  Examples:
    | occupied_start_date | occupied_end_date | start_date  | end_date        |
    | tomorrow           | 2 days from now   | today        | 3 days from now |
    | today               | 3 days from now   | yesterday   | 4 days from now |
    | 2 days from now     | 4 days from now   | tomorrow    | 5 days from now |

# Equivalence Class 9, Start Date: Occupied, End Date: Occupied
  Scenario Outline: Start date occupied, end date occupied
    Given the hotel has 1 rooms available
    And there is an occupied range of dates in the booking system from "<occupied_start_date>" to "<occupied_end_date>"
    And the booking start date is "<start_date>"
    And the booking end date is "<end_date>"
    When I try to create a booking
    Then the booking should fail to be created

  Examples:
    | occupied_start_date | occupied_end_date | start_date  | end_date        |
    | tomorrow           | 3 days from now   | today        | 2 days from now |
    | today               | 4 days from now   | tomorrow    | 3 days from now |
    | 2 days from now     | 5 days from now   | 3 days from now | 4 days from now |

# Edge Case 1, SD Same day as ED
  Scenario Outline: Booking with start date being the same day as end date
    Given the hotel has 1 rooms available
    And there is an occupied range of dates in the booking system from "<occupied_start_date>" to "<occupied_end_date>"
    And the booking start date is "<start_date>"
    And the booking end date is "<end_date>"
    When I try to create a booking
    Then the booking should fail to be created

  Examples:
    | occupied_start_date | occupied_end_date | start_date  | end_date      |
    | today               | 2 days from now   | today        | today        |
    | tomorrow           | 3 days from now   | tomorrow    | tomorrow    |
    | 2 days from now     | 4 days from now   | 3 days from now | 3 days from now |

# Invaid entries
Scenario Outline: Booking with invalid date range
    Given the hotel has 1 rooms available
    And the booking start date is "<start_date>"
    And the booking end date is "<end_date>"
    When I try to create a booking
    Then the booking should fail to be created

Examples:
    | start_date      | end_date        |
    | 2 days from now | tomorrow        |  # End date before start date
    | yesterday       | tomorrow        |  # Start date in the past
    | today          | yesterday      |  # End date in the past

