Feature: Create Booking

# Equivalence Class 1, SD: B, BA: B
    # start date - min
    Scenario: Booking with start date being tomorrow and end date being before the occupied range
        Given the hotel has 1 rooms available
        And there is an occupied range of dates in the booking system from "5 days from now" to "10 days from now"
        And the booking start date is "tomorrow"
        And the booking end date is "2 days after the start date"
        When I try to create a booking
        Then the booking should be created successfully
        
    # start date - max
    Scenario: Booking with start date being 2 days before the occupied range and end date being before the occupied range
        Given the hotel has 1 rooms available
        And there is an occupied range of dates in the booking system from "5 days from now" to "10 days from now"
        And the booking start date is "3 days from now"
        And the booking end date is "1 day after the start date"
        When I try to create a booking
        Then the booking should be created successfully
        
    # end date - min
    Scenario: Booking with start date being before the occupied range and end date being right after the start date
        Given the hotel has 1 rooms available
        And there is an occupied range of dates in the booking system from "5 days from now" to "10 days from now"
        And the booking start date is "2 days from now"
        And the booking end date is "1 day after the start date"
        When I try to create a booking
        Then the booking should be created successfully
        
    # end date - max
    Scenario: Booking with start date being before the occupied range and end date being right before the occupied range
        Given the hotel has 1 rooms available
        And there is an occupied range of dates in the booking system from "5 days from now" to "10 days from now"
        And the booking start date is "2 days from now"
        And the booking end date is "4 days from now"
        When I try to create a booking
        Then the booking should be created successfully
        
# Equivalence Class 2, SD: A, BA: A
    # start date - min
    Scenario: Booking with start date right after the occupied range and end date being after the occupied range
        Given the hotel has 1 rooms available
        And there is an occupied range of dates in the booking system from "5 days from now" to "10 days from now"
        And the booking start date is "11 days from now"
        And the booking end date is "15 days from now"
        When I try to create a booking
        Then the booking should be created successfully
     
    # start date - max
    Scenario: Booking with start date being 2 days before the end of time and end date being after the occupied range (and start date)
        Given the hotel has 1 rooms available
        And there is an occupied range of dates in the booking system from "5 days from now" to "10 days from now"
        And the booking start date is "2 days before the end of time"
        And the booking end date is "1 day before the end of time"
        When I try to create a booking
        Then the booking should be created successfully
        
    # end date - min
    Scenario: Booking with start date being after the occupied range and end date being right after the occupied range (and start date)
        Given the hotel has 1 rooms available
        And there is an occupied range of dates in the booking system from "5 days from now" to "10 days from now"
        And the booking start date is "1 day after the occupied range"
        And the booking end date is "1 day after the start date"
        When I try to create a booking
        Then the booking should be created successfully
        
    # end date - max
    Scenario: Booking with start date being after the occupied range and end date being right before the end of time
        Given the hotel has 1 rooms available
        And there is an occupied range of dates in the booking system from "5 days from now" to "10 days from now"
        And the booking start date is "5 day after the occupied range"
        And the booking end date is "1 day before the end of time"
        When I try to create a booking
        Then the booking should be created successfully
        
    