'use client';
import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { verifyToken } from '@/utils/api';

const useAuth = () => {
    const router = useRouter();

    useEffect(() => {
        const checkToken = async () => {
            const token = localStorage.getItem('jwtToken');

            if (!token) {
                router.push('/login'); // Redirect if no token
                return;
            }

            try 
            {
             await verifyToken() } 
             catch (error) {
                localStorage.clear();
                router.push('/login'); // Redirect if verification fails
            }   
        };

        checkToken();
    }, [router]);
};

export default useAuth;