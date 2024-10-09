'use client';
import React, { useState } from 'react';
import { useParams } from 'next/navigation';
import { CreateBid } from '@/utils/api'; 

const CreateBidForm: React.FC = () => {
    const { id } = useParams();
    const [bidAmount, setBidAmount] = useState<string>(''); // Keep as a string for input handling
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);

    const handleBidSubmit = async (event: React.FormEvent) => {
        event.preventDefault();

        // Convert bidAmount to a number
        const amount = parseFloat(bidAmount);

        // Validation
        if (bidAmount === '' || isNaN(amount)) {
            setError('Bid amount is required and must be a valid number.');
            return;
        }
        if (amount < 0) {
            setError('Bid amount cannot be negative.');
            return;
        }

        const bidData = {
            BidUserId: 'user-id-placeholder', // Replace with actual user ID
            Ammount: amount,
            JobId: parseInt(id as string, 10), // Ensure id is a number
        };

        try {
            await CreateBid(bidData); // Send POST request
            setSuccess('Bid submitted successfully!');
            setBidAmount(''); // Reset bid amount
            setError(null); // Clear any previous errors
        } catch (error) {
            console.log(error)
            setError('Failed to submit bid. Please try again.');
        }
    };

    const handleBidAmountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;

        // Validate input to allow only valid dollar amounts
        const regex = /^\d*\.?\d{0,2}$/; // Allows numbers and up to two decimal places
        if (regex.test(value) || value === '') {
            setBidAmount(value); // Set value as string
            setError(null); // Clear error on valid input
        }
    };

    return (
        <form onSubmit={handleBidSubmit} className="mt-4">
            {error && <p className="text-red-600" role="alert">{error}</p>}
            {success && <p className="text-green-600" role="alert">{success}</p>}
            <div>
                <label htmlFor="bidAmount" className="block text-gray-700">Bid Amount ($):</label>
                <input
                    type="text" // Use text to handle decimal inputs
                    id="bidAmount"
                    value={bidAmount} // Keep it as a string
                    onChange={handleBidAmountChange}
                    className="mt-1 block w-full border border-gray-300 rounded-md p-2"
                    required
                />
            </div>
            <button
                type="submit"
                className="mt-2 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
            >
                Submit Bid
            </button>
        </form>
    );
};

export default CreateBidForm;