import React, { useEffect, useState } from 'react';
import Link from 'next/link';
import { JobDTO } from '../models/types';

interface JobGridProps {
    jobs: JobDTO[];
}

const JobGrid: React.FC<JobGridProps> = ({ jobs }) => {
    const calculateTimeLeft = (expirationDate: string | undefined) => {
        if (!expirationDate) return null;

        const now = new Date().getTime();
        const endTime = new Date(expirationDate).getTime();
        const distance = endTime - now;

        if (distance < 0) return null; // Expired

        const days = Math.floor(distance / (1000 * 60 * 60 * 24));
        const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        const seconds = Math.floor((distance % (1000 * 60)) / 1000);

        return { days, hours, minutes, seconds };
    };

    const [timeLeft, setTimeLeft] = useState<{ [key: number]: { days: number; hours: number; minutes: number; seconds: number } | null }>({});

    useEffect(() => {
        const updateCountdown = () => {
            const newTimeLeft: { [key: number]: { days: number; hours: number; minutes: number; seconds: number } | null } = {};

            jobs.forEach((job) => {
                newTimeLeft[job.id] = calculateTimeLeft(job.expirationDate);
            });

            setTimeLeft(newTimeLeft);
        };

        updateCountdown(); // Initial update
        const interval = setInterval(updateCountdown, 1000); // Update every second

        return () => clearInterval(interval); // Cleanup on unmount
    }, [jobs]);

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6 p-6">
            {jobs.map((job) => (
                <div key={job.id} className="border rounded-lg shadow-lg overflow-hidden bg-white">
                    <div className="p-4">
                        <Link href={`/jobs/${job.id}`}>
                            <h3 className="font-bold text-xl text-gray-800 cursor-pointer hover:underline">
                                {job.description || 'No Description'}
                            </h3>
                        </Link>
                        <p className="text-gray-600 mt-1">Posted by: {job.posterName || 'Anonymous'}</p>
                        <p className="text-gray-600">Created: {new Date(job.createdDate ?? ' ').toLocaleDateString()}</p>
                        <p className="text-gray-600">Expires: {new Date(job.expirationDate ?? ' ').toLocaleDateString()}</p>

                        {timeLeft[job.id] && (
                            <div className="mt-2 text-gray-800">
                                <p className="font-semibold">Time Remaining:</p>
                                <p>
                                    {timeLeft[job.id]?.days}d {timeLeft[job.id]?.hours}h {timeLeft[job.id]?.minutes}m {timeLeft[job.id]?.seconds}s
                                </p>
                            </div>
                        )}
                    </div>
                    <div className="bg-gray-100 p-4">
                        <p className="text-sm text-gray-500">Highest Bid: ${job.highestBid?.toFixed(2) || 'N/A'}</p>
                        <p className="text-sm text-gray-500">Lowest Bid: ${job.lowestBid?.toFixed(2) || 'N/A'}</p>
                        <p className="text-sm text-gray-500">Number of Bids: {job.numberOfBids || 0}</p>
                        <p className="text-sm text-gray-500">Requirements: {job.requirements || 'None'}</p>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default JobGrid;