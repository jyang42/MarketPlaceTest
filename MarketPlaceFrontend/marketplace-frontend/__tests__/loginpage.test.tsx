import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import LoginPage from '../app/login/page'; 
import axios from 'axios';
import { useRouter } from 'next/navigation';
import { useAuth } from '../app/context/AuthContext';

// Mock axios and the useRouter hook
jest.mock('axios');
jest.mock('next/navigation', () => ({
  useRouter: jest.fn(),
}));
jest.mock('../app/context/AuthContext', () => ({
  useAuth: jest.fn(),
}));

describe('LoginPage', () => {
  const mockedAxios = axios as jest.Mocked<typeof axios>;
  const mockRouter = { push: jest.fn() };
  const mockLogIn = jest.fn();

  beforeEach(() => {
    (useRouter as jest.Mock).mockReturnValue(mockRouter);
    (useAuth as jest.Mock).mockReturnValue({ logIn: mockLogIn });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render the login form', () => {
    render(<LoginPage />);

    expect(screen.getByLabelText(/Username/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Password/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /Login/i })).toBeInTheDocument();
  });

  it('should handle successful login', async () => {
    mockedAxios.post.mockResolvedValueOnce({
      data: { token: 'fake-jwt-token' },
    });

    render(<LoginPage />);

    // Fill in the form fields
    fireEvent.change(screen.getByLabelText(/Username/i), {
      target: { value: 'testuser' },
    });
    fireEvent.change(screen.getByLabelText(/Password/i), {
      target: { value: 'password' },
    });

    // Submit the form
    fireEvent.click(screen.getByRole('button', { name: /Login/i }));

    // Wait for the axios post to be called
    await waitFor(() => 
      expect(mockedAxios.post).toHaveBeenCalledWith('http://localhost:32774/api/auth/login', {
        username: 'testuser',
        password: 'password',
      })
    );

    // Check if token is stored in local storage
    expect(localStorage.getItem('jwtToken')).toBe('fake-jwt-token');

    // Check if the logIn function is called
    expect(mockLogIn).toHaveBeenCalled();

    expect(mockRouter.push).toHaveBeenCalledWith('/');
  });

  it('should handle failed login', async () => {
    mockedAxios.post.mockRejectedValueOnce(new Error('Login failed'));

    render(<LoginPage />);

    // Fill in the form fields
    fireEvent.change(screen.getByLabelText(/Username/i), {
      target: { value: 'wronguser' },
    });
    fireEvent.change(screen.getByLabelText(/Password/i), {
      target: { value: 'wrongpassword' },
    });

    // Submit the form
    fireEvent.click(screen.getByRole('button', { name: /Login/i }));

    // Wait for the error message to be displayed
    await waitFor(() => 
      expect(screen.getByText(/Login failed. Please check your credentials./i)).toBeInTheDocument()
    );

    // Ensure that logIn is not called
    expect(mockLogIn).not.toHaveBeenCalled();

    // Ensure that router push is not called
    expect(mockRouter.push).not.toHaveBeenCalled();
  });
});