'use client';
import React, { useEffect, useState } from 'react';
import { JobDTO } from '../../../models/types';
import { useParams } from 'next/navigation';
import useAuth from '../../../hooks/useAuth';
import { fetchJob } from '@/utils/api';
import CreateBidForm from '../../../components/bidform';

const JobDetails: React.FC = () => {
    useAuth();
    const { id } = useParams();
    const [job, setJob] = useState<JobDTO | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [countdown, setCountdown] = useState<string | null>(null);

    useEffect(() => {
        const loadJob = async () => {
            if (!id) return;
            try {
                const data = await fetchJob(id);
                setJob(data);
            } catch (error) {
                console.log(error)
                setError('Failed to load job details');
            } finally {
                setLoading(false);
            }
        };

        loadJob();
    }, [id]);

    useEffect(() => {
        const updateCountdown = () => {
            if (job && job.expirationDate) {
                const expirationDate = new Date(job.expirationDate).getTime();
                const now = Date.now();
                const timeRemaining = expirationDate - now;

                if (timeRemaining > 0) {
                    const hours = Math.floor((timeRemaining % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                    const minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60));
                    const seconds = Math.floor((timeRemaining % (1000 * 60)) / 1000);
                    setCountdown(`${hours}h ${minutes}m ${seconds}s`);
                } else {
                    setCountdown('Expired');
                }
            }
        };

        const interval = setInterval(updateCountdown, 1000);
        return () => clearInterval(interval);
    }, [job]);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>{error}</p>;
    if (!job) return <p>Job not found.</p>;

    return (
        <div className="max-w-md mx-auto p-6 bg-white rounded-lg shadow-md">
            <h1 className="text-2xl font-bold mb-4">{job.description}</h1>
            <p className="text-gray-600"><strong>Posted by:</strong> {job.posterName}</p>
            <p className="text-gray-600"><strong>Requirements:</strong> {job.requirements}</p>
            <p className="text-gray-600"><strong>Highest Bid:</strong> ${job.highestBid?.toFixed(2) || 'N/A'}</p>
            <p className="text-gray-600"><strong>Lowest Bid:</strong> ${job.lowestBid?.toFixed(2) || 'N/A'}</p>
            <p className="text-gray-600"><strong>Created:</strong> {new Date((job.createdDate) ?? '').toLocaleDateString()}</p>
            <p className="text-gray-600"><strong>Expires:</strong> {job.expirationDate ? new Date(job.expirationDate).toLocaleDateString() : 'N/A'}</p>
            <p className="text-red-600 font-bold mt-4"><strong>Time Remaining:</strong> {countdown}</p>
            
            {/* Include the CreateBidForm */}
            {job && countdown !== 'Expired' && (
                <CreateBidForm />
            )}
            
            {/* Display Bids in a Grid */}
            <div className="mt-6">
                <h2 className="text-xl font-bold mb-2">Bids</h2>
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                    {job.bids && job.bids.length > 0 ? (
                        job.bids.map((bid) => (
                            <div key={bid.id} className="p-4 border rounded shadow-sm">
                                <p><strong>Bidder:</strong> {bid.bidderName}</p>
                                <p><strong>Amount:</strong> ${(bid.ammount ?? 0).toFixed(2)}</p>
                                <p><strong>Date:</strong> {new Date((bid.createdDate ?? ' ')).toLocaleDateString()}</p>
                            </div>
                        ))
                    ) : (
                        <p>No bids available for this job.</p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default JobDetails;