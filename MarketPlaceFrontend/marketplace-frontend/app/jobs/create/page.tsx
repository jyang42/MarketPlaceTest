'use client';

import React, { useState } from 'react';
import useAuth from '../../../hooks/useAuth'
import { useRouter } from 'next/navigation';
import { CreateJob } from '@/utils/api';

interface CreateJobDTO {
    description?: string;
    requirements?: string;
    createdDate: string; // Use string for input value
    expirationDate?: string; // Optional
    posterId?: string; // Optional
}

const CreateJobForm: React.FC = () => {
    useAuth()
    const router = useRouter();
    const [job, setJob] = useState<CreateJobDTO>({
        description: '',
        requirements: '',
        createdDate: new Date().toISOString().split('T')[0], // Default to today
        expirationDate: '',
        posterId: '' // Set as needed
    });

    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<boolean>(false);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setJob((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setError(null);
        setSuccess(false);

        try {
            const data = await CreateJob(job)
            const i = data.id
            setSuccess(true);
            setJob({ description: '', requirements: '', createdDate: new Date().toISOString().split('T')[0], expirationDate: '', posterId: '' }); // Reset form
            router.push('/jobs/' + i)

        } catch (error) {
            console.log(error)
            setError('Failed to create job');
        }
    };

    return (
        <div className="max-w-lg mx-auto p-6 bg-white rounded-lg shadow-lg">
            <h1 className="text-2xl font-bold text-center mb-4">Create Job</h1>
            <form onSubmit={handleSubmit}>
                <div className="mb-4">
                    <label className="block text-gray-700 mb-2">Description:</label>
                    <textarea
                        name="description"
                        value={job.description}
                        onChange={handleChange}
                        required
                        className="border border-gray-300 rounded-lg p-2 w-full h-24 focus:outline-none focus:ring-2 focus:ring-blue-500"
                    />
                </div>
                <div className="mb-4">
                    <label className="block text-gray-700 mb-2">Requirements:</label>
                    <textarea
                        name="requirements"
                        value={job.requirements}
                        onChange={handleChange}
                        required
                        className="border border-gray-300 rounded-lg p-2 w-full h-24 focus:outline-none focus:ring-2 focus:ring-blue-500"
                    />
                </div>
                <div className="mb-4">
                    <label className="block text-gray-700 mb-2">Created Date:</label>
                    <input
                        type="date"
                        name="createdDate"
                        value={job.createdDate}
                        onChange={handleChange}
                        required
                        className="border border-gray-300 rounded-lg p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500"
                    />
                </div>
                <div className="mb-4">
                    <label className="block text-gray-700 mb-2">Expiration Date:</label>
                    <input
                        type="date"
                        name="expirationDate"
                        value={job.expirationDate || ''}
                        onChange={handleChange}
                        className="border border-gray-300 rounded-lg p-2 w-full focus:outline-none focus:ring-2 focus:ring-blue-500"
                    />
                </div>
                <button type="submit" className="w-full bg-blue-500 text-white rounded-lg px-4 py-2 hover:bg-blue-600 transition duration-200">Submit</button>
            </form>

            {error && <p className="text-red-500 text-center mt-4">{error}</p>}
            {success && <p className="text-green-500 text-center mt-4">Job created successfully!</p>}
        </div>
    );
};

export default CreateJobForm;