Feature: Users API

    Background:
        Given the Users API is available

    @API
    @Scenario1
    Scenario: Successfully retrieve users with valid page parameter
        When I request users for page 1
        Then the response should be successful
        And the users list should contain 6 users
        And the page number should be 1

    @API
    @Scenario2
    Scenario Outline: Test different page numbers
        When I request users for page <page>
        Then the response should be successful
        And the page number should be <page>
        And the users list should contain <userCount> users

        Examples:
          | page | userCount |
          | 1    | 6         |
          | 2    | 6         |

    @API
    @Scenario3
    Scenario: Handle invalid page parameter
        When I request users for page 999
        Then the response should be successful
        And the users list should contain 0 users

    @API
    @Scenario4
    Scenario: Verify specific user data
        When I request users for page 1
        Then the response should be successful
        And the response should contain a user with:
          | First_Name | Last_Name | Email                  | Avatar                                  |
          | Janet      | Weaver    | janet.weaver@reqres.in | https://reqres.in/img/faces/2-image.jpg |

    @API
    @Scenario5
    Scenario Outline: Verify specific user data with fixtures
        When I request users for page <page>
        Then the response should be successful
        And the response should contain the user from <data sample>

        Examples:
          | page | data sample           |
          | 1    | userResponseData_Page1.json |
          | 2    | userResponseData_Page2.json |