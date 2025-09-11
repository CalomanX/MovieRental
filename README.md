# MovieRental Exercise

This is a dummy representation of a movie rental system.
Can you help us fix some issues and implement missing features?

 * The app is throwing an error when we start, please help us. Also, tell us what caused the issue.
 
    FIX builder.Services.AddSingleton<IRentalFeatures, RentalFeatures>();
    
    Dependency lifetime mismatch
    
    Singleton(Scoped) is a lifetime mismatch as singleton live forever and scoped (in WEB) is aligned with the request context.
    Specially with DBContext which is registered as Scoped by default, memmory leaks and other issues may occur.  
    
    Change the IRentalFeatures to Scoped, or if it MUST be a Singleton, use IDbContextFactory.

 * The rental class has a method to save, but it is not async, can you make it async and explain to us what is the difference?
 
    FIX: Make this method async
    
    Blocking calls can be a source of performance issues specially in heavy load APIs.
    
    Why async? In (very) simple terms its like you have this fast-food restaurant with 5 frontend employees (available threads) and a lot of clients (making requests) in a line. 
    Without async, the first client make a request and an available employee (thread from the pool) will make all the work for that client. 
    He will wait for the cook to prepare the meal (blocking call) and then after that deliver the meal to the client. Only now he can take another request;
    Easy to see that somewhere along the line all waiters will be full and no more clients can be served (thread starvation - all threads busy waiting).
    
    With async, the first client make a request and the first employee (thread from the pool) will take the request and ask the cook to prepare the meal (non-blocking call).
    Then will go back to the line and take another request. When the meal is ready, the cook will call the employee to deliver the meal to the client.
    Thread starvation is now a lot less likely to happen as the waiter is not locked waiting for the cook to prepare the meal.
    
    Async/await is just sugar coating for callbacks and state machines, running on old tech (TPL) but it makes life a LOT easyer.
	

 * Please finish the method to filter rentals by customer name, and add the new endpoint.
 * We noticed we do not have a table for customers, it is not good to have just the customer name in the rental.
   Can you help us add a new entity for this? Don't forget to change the customer name field to a foreign key, and fix your previous method!
 * In the MovieFeatures class, there is a method to list all movies, tell us your opinion about it.
 
 * No exceptions are being caught in this api, how would you deal with these exceptions?

    For business rule exceptions, create custom exceptions and catch them in the controller returning a proper status code (400, 409, custom...).
    For system exceptions, create a global exception handler middleware to catch unhandled exceptions and log them properly, returning a 500 status code.

	## Challenge (Nice to have)
We need to implement a new feature in the system that supports automatic payment processing. Given the advancements in technology, it is essential to integrate multiple payment providers into our system.

Here are the specific instructions for this implementation:

* Payment Provider Classes:
    * In the "PaymentProvider" folder, you will find two classes that contain basic (dummy) implementations of payment providers. These can be used as a starting point for your work.
* RentalFeatures Class:
    * Within the RentalFeatures class, you are required to implement the payment processing functionality.
* Payment Provider Designation:
    * The specific payment provider to be used in a rental is specified in the Rental model under the attribute named "PaymentMethod".
* Extensibility:
    * The system should be designed to allow the addition of more payment providers in the future, ensuring flexibility and scalability.
* Payment Failure Handling:
    * If the payment method fails during the transaction, the system should prevent the creation of the rental record. In such cases, no rental should be saved to the database.
