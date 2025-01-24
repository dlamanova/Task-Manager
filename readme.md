# Task Manager

Task Manager is a web application that allows administrators to manage tasks and projects effectively. This platform supports creating tasks, assigning them to users, categorizing them, and organizing them under projects. It features a drag-and-drop interface for task management, a modal system for creating and editing entities, and user-friendly navigation.

---

## Features

- **Task Management**: Create, update, and delete tasks.
- **Project Management**: Create projects with multiple tasks.
- **User Roles**: Role-based access control with Admin and Regular User roles.
- **Drag-and-Drop Interface**: Organize tasks between columns (e.g., TODO, In Progress, Done).
- **Dynamic Modals**: Add and edit tasks and projects dynamically through modals.

---

## Installation

Follow these steps to install and run the Task Manager project:

### Prerequisites

1. **Operating System**: Windows/Linux/MacOS
2. **Development Environment**:
   - .NET SDK (6.0 or later)
   - Node.js (for managing frontend dependencies, if required)
   - SQL Server
3. **Tools**:
   - Visual Studio or Visual Studio Code
   - Git
   - A web browser (Chrome, Firefox, etc.)

### Steps

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-repository/task-manager.git
   cd task-manager
   ```

2. **Restore NuGet Packages**:
   Run the following command to restore required dependencies:
   ```bash
   dotnet restore
   ```

3. **Update the Database Connection**:
   Open `appsettings.json` and configure the connection string for your SQL Server instance:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=TaskManagerDb;Trusted_Connection=True;"
   }
   ```

4. **Apply Migrations**:
   Run the following command to apply database migrations and seed the database:
   ```bash
   dotnet ef database update
   ```

5. **Run the Application**:
   Start the application with:
   ```bash
   dotnet run
   ```
   The application will be available at `http://localhost:5000` or `https://localhost:5001`.

6. **Login Credentials**:
   Use the seeded admin credentials to log in:
   - **Username**: admin
   - **Password**: Admin123!

---

## Using the Application

### Logging In
- Navigate to the login page (`/login`).
- Use the provided credentials or create a new account if registration is enabled.

### Managing Tasks
1. Navigate to the "Tasks" section.
2. Use the **Create Task** button to open the modal for creating a task.
3. Fill in the task details, including name, description, category, and deadline.
4. Save the task, and it will appear in the appropriate column (TODO, In Progress, Done).
5. Drag and drop tasks between columns to update their statuses.

### Managing Projects
1. Navigate to the "Projects" section.
2. Use the **Create Project** button to open the project creation modal.
3. Enter the project details, including the name and description.
4. Add tasks to the project dynamically by clicking the **Add Task** button.
5. Save the project, and it will appear in the Projects column.

### Editing Tasks and Projects
- Double-click on a task or project to view more details and edit them.
- Save changes, and they will be reflected immediately.

### Deleting Tasks
- Select the task you want to delete and confirm the deletion in the modal or UI.

---

## Contributing

1. **Fork the Repository**:
   - Click the "Fork" button on the repository page.

2. **Clone the Fork**:
   ```bash
   git clone https://github.com/your-username/task-manager.git
   ```

3. **Create a Branch**:
   ```bash
   git checkout -b feature/your-feature-name
   ```

4. **Make Changes and Commit**:
   ```bash
   git add .
   git commit -m "Add your feature description here"
   ```

5. **Push the Changes**:
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Create a Pull Request**:
   - Open a pull request from your fork's branch to the main repository.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Contact

For any queries, please contact:
- **Email**: support@taskmanager.com
- **GitHub Issues**: [Issue Tracker](https://github.com/your-repository/task-manager/issues)

